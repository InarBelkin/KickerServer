using BLL.Dtos.Lobby;

namespace BLL.Interfaces;

public interface IBattleService
{
    Task<Guid> AddBattle(LobbyItemM lobby);
    Task<LobbyItemM?> GetBattle(Guid id);
}