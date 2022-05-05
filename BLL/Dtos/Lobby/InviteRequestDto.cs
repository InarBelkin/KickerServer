namespace BLL.Dtos.Lobby;

public class InviteRequestDto
{
    public string InvitedId { get; set; } = string.Empty;
    public string? Message { get; set; }
}