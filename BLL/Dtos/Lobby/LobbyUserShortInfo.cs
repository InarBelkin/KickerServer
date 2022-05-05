using GeneralLibrary.Enums;

namespace BLL.Dtos.Lobby;

public class LobbyUserShortInfo
{
    public Guid? Id { get; set; }
    public Role Role { get; set; }
    public IsAccepted Accepted { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; }
}