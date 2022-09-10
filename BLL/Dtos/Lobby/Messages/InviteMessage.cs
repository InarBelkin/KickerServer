namespace BLL.Dtos.Lobby.Messages;

public class InviteMessage
{
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public Guid InvitedId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsInviteToAll { get; set; } = false;
    public int Side { get; set; }
    public int Position { get; set; }
}