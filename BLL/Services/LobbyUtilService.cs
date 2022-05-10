using BLL.Dtos.Lobby;
using BLL.Dtos.Messages;
using DAL.Repositories;
using GeneralLibrary.Enums;

namespace BLL.Services;

public class LobbyUtilService : ILobbyUtilService
{
    private IConnectedUsersRepository _connectedUsersRepository;
    private readonly ILobbyRepository _lobbyRepository;


    public LobbyUtilService(IConnectedUsersRepository connectedUsersRepository, ILobbyRepository lobbyRepository)
    {
        _connectedUsersRepository = connectedUsersRepository;
        _lobbyRepository = lobbyRepository;
    }

    public async Task AddUserOnline(Guid id)
    {
        await _connectedUsersRepository.AddOnlineUser(id);
    }

    public async Task DeleteUserOnline(Guid id)
    {
        await _connectedUsersRepository.DeleteOnlineUser(id);
    }

    public async Task<Dictionary<Guid, UserStatus>> GetUserStatuses()
    {
        var ret = new Dictionary<Guid, UserStatus>();

        foreach (var onlineUser in await _connectedUsersRepository.GetOnlineUsers())
        {
            ret[onlineUser] = UserStatus.Online;
        }

        var lobbys = await _lobbyRepository.GetLobbyList();
        var inBattleUsers = lobbys.SelectMany(l =>
            l.SideA.Concat(l.SideB).Where(s => s.Id != null && s.Accepted == IsAccepted.Accepted)
                .Select(s => s.Id!.Value).Concat(new[] {l.Initiator}));
        foreach (var battleUser in inBattleUsers)
        {
            ret[battleUser] = UserStatus.InBattle;
        }

        return ret;
    }
}