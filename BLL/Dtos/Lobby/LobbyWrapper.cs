namespace BLL.Dtos.Lobby;

public class LobbyWrapper
{
    public List<LobbyItemM> LobbyItems { get; set; } = new();
    public LobbyItemM? CurrentLobby { get; set; } = null;
}