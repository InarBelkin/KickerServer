using GeneralLibrary.Enums;

namespace DAL.InMemoryEntities.Lobby;

public class LobbyTimeStamp
{
    public BattleStatus BattleState { get; set; }
    public DateTime GlobalTime { get; set; }
    public double BattleTime { get; set; }
}