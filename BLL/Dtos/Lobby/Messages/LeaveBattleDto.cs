namespace BLL.Dtos.Lobby.Messages;

public class LeaveBattleDto
{
    public Guid InvitedId { get; set; }
    public Guid InitiatorId { get; set; }
}