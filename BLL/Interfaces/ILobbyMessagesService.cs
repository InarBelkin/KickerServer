using BLL.Dtos.Lobby;
using BLL.Dtos.Lobby.Messages;
using BLL.Dtos.Messages;

namespace BLL.Services;

public interface ILobbyMessagesService
{
    Task InviteOne(InviteMessage message, Guid InvitedId);
    Task InviteAll(InviteMessage message);
    Task AnswerToInvite(InviteAnswer answer);
    Task YourLobbyWasUpdated(LobbyItemM lobbyItem);
    Task<MessageBaseDto> LeaveBattle(LeaveBattleDto dto);
    Task YourLobbyWasDeleted(bool withResults, Guid battleId, Guid userId);
    Task<MessageBaseDto> EndBattleEarly(Guid initiatorId);
}