using DAL.InMemoryEntities.Lobby;

namespace DAL.Repositories;

public interface ILobbyRepository
{
    Task<List<LobbyItem>> GetLobbyList();
    Task<bool> AddLobbyItem(LobbyItem lobbyItem);
    Task<bool> DeleteItem(Guid initiatorId);
    Task<bool> UpdateItem(LobbyItem item);
    Task<LobbyItem?> GetLobbyByInitiator(Guid InitiatorId);
}