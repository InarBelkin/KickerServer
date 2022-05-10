using GeneralLibrary.Enums;
using static System.String;

namespace DAL.InMemoryEntities.Lobby;

public class LobbyItem
{
    public LobbyTimeStamp[] TimeStamps { get; set; } = Array.Empty<LobbyTimeStamp>();
    public LobbyResult Result { get; set; } = new();

    public DateTime? DateStart { get; set; }
    public string Message { get; set; } = Empty;
    public Guid Initiator { get; set; }

    public List<LobbyUser> SideA { get; set; } = new();
    public List<LobbyUser> SideB { get; set; } = new();
}