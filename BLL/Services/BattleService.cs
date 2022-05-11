using BLL.Dtos.Lobby;
using BLL.Interfaces;
using DAL.Entities.Battle;
using DAL.InMemoryEntities.Lobby;
using GeneralLibrary.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class BattleService : ServiceBasePg, IBattleService
{
    public BattleService(IHttpContextAccessor accessor) : base(accessor)
    {
    }

    public async Task<Guid> AddBattle(LobbyItemM lobby)
    {
        var battle = new Battle()
        {
            Message = lobby.Message,
            BattleTime = lobby.TimeStamps.Last().GlobalTime,
            BattleTimeSeconds = lobby.TimeStamps.Last().BattleTime,
            IsWinnerA = lobby.Result.IsWinnerA!.Value,
            LoserGoalsCount = lobby.Result.CountOfGoalsLoser
        };

        battle.BattleTime = DateTime.SpecifyKind(battle.BattleTime, DateTimeKind.Utc);

        for (int i = 0; i < lobby.SideA.Count; i++)
            if (lobby.SideA[i].Id != null && lobby.SideA[i].Accepted == IsAccepted.Accepted)
                battle.UserBattles.Add(new()
                {
                    Side = 0, Role = i, UserId = lobby.SideA[i].Id!.Value,
                    IsInitiator = lobby.SideA[i].Id!.Value == lobby.Initiator.Id
                });

        for (int i = 0; i < lobby.SideB.Count; i++)
            if (lobby.SideB[i].Id != null && lobby.SideB[i].Accepted == IsAccepted.Accepted)
                battle.UserBattles.Add(new()
                {
                    Side = 1, Role = i, UserId = lobby.SideB[i].Id!.Value,
                    IsInitiator = lobby.SideB[i].Id!.Value == lobby.Initiator.Id
                });

        Db.Battles.Add(battle);
        await Db.SaveChangesAsync();
        return battle.Id;
    }

    public async Task<LobbyItemM> GetBattle(Guid id)
    {
        var battle = await Db.Battles.Include(b => b.Users).FirstOrDefaultAsync(b => b.Id == id);

        
        var lobby = new LobbyItemM(battle!);
        foreach (var lobbyTimeStampM in lobby.TimeStamps)
        {
            lobbyTimeStampM.GlobalTime = DateTime.SpecifyKind(lobbyTimeStampM.GlobalTime, DateTimeKind.Unspecified);
        }

        return lobby;
    }
}