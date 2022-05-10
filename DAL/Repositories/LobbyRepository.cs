using System.Text.Json;
using DAL.InMemoryEntities.Lobby;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DAL.Repositories;

public class LobbyRepository : ILobbyRepository
{
    private readonly IConnectionMultiplexer _multiplexer;

    private IDatabase Db { get; }

    //private const string LobbyListKey = "LobbyList";
    private const string LobbyHashKey = "LobbyHash";

    public LobbyRepository(ConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        Db = _multiplexer.GetDatabase()!;
    }

    public async Task<List<LobbyItem>> GetLobbyList()
    {
        var lobbysString = (await Db.HashValuesAsync(LobbyHashKey))
            .Select(s => JsonSerializer.Deserialize<LobbyItem>(s)!).ToList();
        return lobbysString;
    }

    public async Task<LobbyItem?> GetLobbyByInitiator(Guid InitiatorId)
    {
        var lobbyString = await Db.HashGetAsync(LobbyHashKey, InitiatorId.ToString());
        return JsonSerializer.Deserialize<LobbyItem>(lobbyString);
    }

    public async Task<bool> AddLobbyItem(LobbyItem lobbyItem)
    {
        if (await Db.HashExistsAsync(LobbyHashKey, lobbyItem.Initiator.ToString())) return false;
        var valueString = JsonSerializer.Serialize(lobbyItem);
        return await Db.HashSetAsync(LobbyHashKey, lobbyItem.Initiator.ToString(), valueString);
    }

    public async Task<bool> DeleteItem(Guid initiatorId) =>
        await Db.HashDeleteAsync(LobbyHashKey, initiatorId.ToString());

    public async Task DeleteAll()
    {
        await Db.KeyDeleteAsync(LobbyHashKey);
    }

    public async Task<bool> UpdateItem(LobbyItem item)
    {
        var valueString = JsonSerializer.Serialize(item);
        return await Db.HashSetAsync(LobbyHashKey, item.Initiator.ToString(), valueString);
    }
}