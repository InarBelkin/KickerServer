using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;

namespace BLL.Services.Hubs;

public interface ITypedInvitationHub
{
    Task Invite(InviteMessage message);
    Task AnswerInvite(InviteAnswer answer);

    Task YourLobbyChanged(string lobbyItem);
    Task YourLobbyDeleted(bool withResults, string BattleId);
}