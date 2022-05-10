using GeneralLibrary.Enums;

namespace BLL.Services;

public interface ILobbyUtilService
{
    Task AddUserOnline(Guid id);
    Task DeleteUserOnline(Guid id);
    Task<Dictionary<Guid, UserStatus>> GetUserStatuses();
}