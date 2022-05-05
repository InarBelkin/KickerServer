using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;

namespace BLL.Services;

public interface ILobbyMessagesService
{
    Task InviteOne(InviteMessage message, Guid InvitedId);
    Task InviteAll(InviteMessage message);
    Task AnswerToInvite(InviteAnswer answer);
    Task YourLobbyWasUpdated(LobbyItemM lobbyItem);
}