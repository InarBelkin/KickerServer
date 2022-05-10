namespace DAL.Repositories;

public interface IConnectedUsersRepository
{
    Task AddOnlineUser(Guid id);
    Task<HashSet<Guid>> GetOnlineUsers();
    Task DeleteOnlineUser(Guid id);
}