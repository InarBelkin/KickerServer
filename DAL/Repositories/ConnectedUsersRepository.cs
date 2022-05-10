using StackExchange.Redis;

namespace DAL.Repositories;

public class ConnectedUsersRepository : IConnectedUsersRepository
{
    private readonly ConnectionMultiplexer _multiplexer;
    private IDatabase Db { get; }

    private const string Key = "ConnectedUsers";

    public ConnectedUsersRepository(ConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        Db = _multiplexer.GetDatabase()!;
    }

    public async Task AddOnlineUser(Guid id)
    {
        var sid = id.ToString();
        await Db.HashSetAsync(Key, sid, sid);
    }

    public async Task DeleteOnlineUser(Guid id)
    {
        var sid = id.ToString();
        await Db.HashDeleteAsync(Key, sid);
    }

    public async Task<HashSet<Guid>> GetOnlineUsers()
    {
        var rez = await Db.HashValuesAsync(Key);
        return rez.Select(v => Guid.Parse(v)).ToHashSet();
    }
}