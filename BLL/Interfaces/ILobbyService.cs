using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Dtos.Messages;

namespace BLL.Interfaces;

public interface ILobbyService
{
    Task<List<LobbyItemM>> GetLobbyList();

    Task<MessageBaseDto> DeleteLobby(Guid userId);
    Task<MessageBaseDto> StartLobby(LobbyItemM item);
    Task<MessageBaseDto> UpdateLobby(LobbyItemM item);
    Task<LobbyItemM?> GetMyLobby();
    Task<HashSet<Guid>> GetAllPlayingUsers();
    Task<MessageBaseDto> ApplyUserInviteAnswer(InviteAnswer answer);
}