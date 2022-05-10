using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Dtos.Messages;
using BLL.Dtos.Stats;
using BLL.Interfaces;
using DAL.InMemoryEntities.Lobby;
using DAL.Repositories;
using GeneralLibrary.Enums;
using GeneralLibrary.Extensions;
using StackExchange.Redis;
using LobbyItemM = BLL.Dtos.Lobby.LobbyItemM;

namespace BLL.Services;

public class LobbyService : ILobbyService
{
    private readonly ILobbyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IStatsService _statsService;
    private readonly IAuthService _authService;
    private readonly ILobbyMessagesService _lobbyMessagesService;
    private readonly IBattleService _battleService;

    public LobbyService(ILobbyRepository repository, IMapper mapper, IStatsService statsService,
        IAuthService authService, ILobbyMessagesService lobbyMessagesService, IBattleService battleService)
    {
        _repository = repository;
        _mapper = mapper;
        _statsService = statsService;
        _authService = authService;
        _lobbyMessagesService = lobbyMessagesService;
        _battleService = battleService;
    }


    public async Task<List<LobbyItemM>> GetLobbyList()
    {
        var lobbyList = await _repository.GetLobbyList();
        var ret = new List<LobbyItemM>(lobbyList.Count);

        var user = _authService.GetUserClaims();

        foreach (var l in lobbyList)
        {
            bool iAmMember;
            if (user == null)
                iAmMember = false;
            else
                iAmMember =
                    (l.SideA.Concat(l.SideB).Count(lb => lb.Accepted == IsAccepted.Accepted && lb.Id == user.Id) > 0 ||
                     l.Initiator == user.Id);

            ret.Add(await MapLobbyM(l, iAmMember));
        }

        return ret;
    }

    public async Task<LobbyItemM?> GetMyLobby()
    {
        var user = _authService.GetUserClaims();
        if (user == null) return null;

        var lobbyList = await _repository.GetLobbyList();
        var lobby = lobbyList
            .FirstOrDefault(l => l.Initiator == user.Id ||
                                 l.SideA.Concat(l.SideB).Count(lb =>
                                     lb.Accepted == IsAccepted.Accepted && lb.Id == user.Id) > 0);
        return lobby == null ? null : await MapLobbyM(lobby, true);
    }

    public async Task<LobbyItemM?> GetLobbyByInitiator(Guid initiatorId)
    {
        var lobby = await _repository.GetLobbyByInitiator(initiatorId);
        if (lobby == null) return null;
        return await MapLobbyM(lobby, true);
    }

    private async Task<LobbyItemM> MapLobbyM(LobbyItem lobby, bool member) =>
        new()
        {
            TimeStamps = lobby.TimeStamps.Select(t => new LobbyTimeStampM(t)).ToArray(),
            Result = new LobbyResultM(lobby.Result),

            DateStart = lobby.DateStart,
            IAmMember = member,
            Message = lobby.Message,
            Initiator = (await _statsService.GetUserShortInfoPartial(lobby.Initiator))!,
            SideA = await lobby.SideA.Select(async a => await GetLobbyUserShortInfo(a)).SequenceAwait(),
            SideB = await lobby.SideB.Select(async a => await GetLobbyUserShortInfo(a)).SequenceAwait(),
        };

    private async Task<LobbyUserShortInfo> GetLobbyUserShortInfo(LobbyUser lobbyUser)
    {
        var user = await _statsService.GetUserShortInfoPartial(lobbyUser.Id) ?? new LobbyUserShortInfo();

        user.Id = lobbyUser.Id;
        user.Role = lobbyUser.Role;
        user.Accepted = lobbyUser.Accepted;

        return user;
    }


    public async Task<MessageBaseDto> StartLobby(LobbyItemM item)
    {
        if (item.Initiator.Id == null) return new() {Success = false, Message = "initiator was null"};

        var lobby = _mapper.Map<LobbyItem>(item);
        var success = await _repository.AddLobbyItem(lobby);
        return new MessageBaseDto()
        {
            Success = success,
            Message = success ? "Lobby was created" : "Your battle already existed"
        };
    }

    public async Task<MessageBaseDto> UpdateLobby(LobbyItemM item) //TODO: add validation (users must be not repeated)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(item);

        var rez = Validator.TryValidateObject(item, context, results);
        if (rez)
        {
            item.SideA.Concat(item.SideB).ToList().ForEach(u =>
            {
                if (u.Accepted is IsAccepted.Empty or IsAccepted.AllInvited) u.Id = null;
            });

            var lobby = _mapper.Map<LobbyItem>(item);
            var success = await _repository.UpdateItem(lobby);
            await _lobbyMessagesService.YourLobbyWasUpdated(await MapLobbyM(lobby, false));
            return new MessageBaseDto()
            {
                Success = true,
                Message = "Lobby was successfully updated"
            };
        }
        else
        {
            await _lobbyMessagesService.YourLobbyWasUpdated(item);
            return new MessageBaseDto()
            {
                Success = false,
                Message = "Validation error:\n" + string.Join("\n", results)
            };
        }
    }


    public async Task<MessageBaseDto> DeleteLobby(Guid userId)
    {
        var success = await _repository.DeleteItem(userId);

        return new MessageBaseDto()
        {
            Success = success,
            Message = success ? "Lobby was deleted" : "Lobby wasn't existed"
        };
    }

    public async Task DeleteAll()
    {
        await _repository.DeleteAll();
    }

    public async Task<MessageBaseDto> ApplyUserInviteAnswer(InviteAnswer answer)
    {
        var lobby = await GetLobbyByInitiator(answer.InitiatorId);
        if (lobby != null)
        {
            var side = answer.Side == 0 ? lobby.SideA : lobby.SideB;
            var user = side[answer.Position];

            switch (user.Accepted)
            {
                case IsAccepted.Invited when user.Id == answer.InvitedId:
                    side[answer.Position].Accepted = answer.Accepted ? IsAccepted.Accepted : IsAccepted.Refused;
                    var message = await UpdateLobby(lobby);
                    return message;
                case IsAccepted.Invited:
                    return new MessageBaseDto()
                    {
                        Success = false,
                        Message = "This place is not for you, molodoy thzelovek"
                    };
                case IsAccepted.AllInvited when answer.Accepted == true:
                    side[answer.Position].Id = answer.InvitedId;
                    side[answer.Position].Accepted = IsAccepted.Accepted;
                    return await UpdateLobby(lobby);
                case IsAccepted.AllInvited:
                    return new MessageBaseDto()
                    {
                        Success = false,
                        Message = "It's don't necessary to apply cancel invite to all"
                    };
            }
        }

        return new MessageBaseDto()
        {
            Success = false,
            Message = "Lobby doesn't exists"
        };
    }

    public async Task<MessageBaseDto> ApplyLeaveBattle(LeaveBattleDto dto)
    {
        var lobby = await GetLobbyByInitiator(dto.InitiatorId);
        if (lobby != null)
        {
            var user = lobby.SideA.Concat(lobby.SideB).FirstOrDefault(u => u.Id == dto.InvitedId);
            if (user is {Accepted: IsAccepted.Accepted})
            {
                user.Accepted = IsAccepted.Left;
                var message = await UpdateLobby(lobby);
                return message;
            }

            return new MessageBaseDto()
            {
                Success = false,
                Message = "You are trying to left the battle, but you're not in it or place is right"
            };
        }

        return new MessageBaseDto()
        {
            Success = false,
            Message = "Lobby doesn't exists"
        };
    }

    public async Task<BattleAnswerDto> EndOfBattle(LobbyItemM lobby)
    {
        if (lobby.Result.IsWinnerA == null) return new() {Success = false, Message = "winner must be selected"};

        if (lobby.SideA.All(u => u.Id != null) &&
            lobby.SideB.All(u => u.Id != null && u.Accepted == IsAccepted.Accepted))
            await _statsService.ApplyLobbyStats(lobby);

        var battleId = await _battleService.AddBattle(lobby);

        foreach (var user in lobby.SideA.Concat(lobby.SideB).Where(u => u.Id != null))
            await _lobbyMessagesService.YourLobbyWasDeleted(true, battleId, user.Id!.Value);

        await DeleteLobby(lobby.Initiator.Id!.Value);

        return new() {Success = true, Message = "battle results was written", BattleId = battleId};
    }
}