using System.Text.Json;
using System.Text.Json.Serialization;
using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Dtos.Messages;
using BLL.Interfaces;
using BLL.Services.Hubs;
using BLL.Util;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public class LobbyMessagesService : ILobbyMessagesService
{
    protected HttpContext HttpContext { get; }
    private readonly IHubContext<InvitationHub, ITypedInvitationHub> _hub;

    protected ILobbyService LobbyService => _lobbyService ??= HttpContext.RequestServices.GetService<ILobbyService>()!;
    private ILobbyService? _lobbyService;


    public LobbyMessagesService(IHubContext<InvitationHub, ITypedInvitationHub> invitationHubContext,
        IHttpContextAccessor accessor)
    {
        HttpContext = accessor.HttpContext!;
        _hub = invitationHubContext;
    }

    public async Task InviteOne(InviteMessage message, Guid InvitedId)
    {
        await _hub.Clients.User(InvitedId.ToString()).Invite(message);
    }

    public async Task InviteAll(InviteMessage message)
    {
        await _hub.Clients.All.Invite(message);
    }

    public async Task AnswerToInvite(InviteAnswer answer)
    {
        await LobbyService.ApplyUserInviteAnswer(answer);
        await _hub.Clients.User(answer.InitiatorId.ToString()).AnswerInvite(answer);
    }

    public async Task YourLobbyWasUpdated(LobbyItemM lobbyItem)
    {
        foreach (var user in lobbyItem.SideA.Concat(lobbyItem.SideB).Concat(new[] {lobbyItem.Initiator})
                     .Where(u => u.Id != null))
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(lobbyItem, serializeOptions);

            await _hub.Clients.User(user.Id.ToString()!).YourLobbyChanged(json);
        }
    }

    public async Task YourLobbyWasDeleted(bool withResults, Guid battleId, Guid userId)
    {
        await _hub.Clients.User(userId.ToString()!).YourLobbyDeleted(withResults, battleId.ToString());
    }

    public async Task<MessageBaseDto> LeaveBattle(LeaveBattleDto dto)
    {
        return await LobbyService.ApplyLeaveBattle(dto);
    }

    public async Task<MessageBaseDto> EndBattleEarly(Guid initiatorId)
    {
        var lobby = await LobbyService.GetLobbyByInitiator(initiatorId);
        if (lobby == null) return new() {Message = "batlle already doesnt exists", Success = false};

        foreach (var user in lobby.GetAllUsers().Where(u => u.Id != null))
            await _hub.Clients.User(user.Id!.Value.ToString()).YourLobbyDeleted(false, "");
        return await LobbyService.DeleteLobby(initiatorId);
    }
}