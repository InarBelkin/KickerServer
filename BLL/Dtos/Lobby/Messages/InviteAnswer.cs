namespace BLL.Dtos.Lobby.Messages;

public class InviteAnswer
{
    public Guid InvitedId { get; set; }
    public Guid InitiatorId { get; set; }
    public bool Accepted { get; set; }
    public int Side { get; set; }
    public int Position { get; set; }
}