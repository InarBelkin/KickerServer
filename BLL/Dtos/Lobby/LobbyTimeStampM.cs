using DAL.InMemoryEntities.Lobby;
using GeneralLibrary.Enums;

namespace BLL.Dtos.Lobby;

public class LobbyTimeStampM
{
    public LobbyTimeStampM()
    {
    }

    public LobbyTimeStampM(LobbyTimeStamp lobbyTimeStamp)
    {
        BattleState = lobbyTimeStamp.BattleState;
        GlobalTime = lobbyTimeStamp.GlobalTime;
        BattleTime = lobbyTimeStamp.BattleTime;
    }

    public BattleStatus BattleState { get; set; }
    public DateTime GlobalTime { get; set; }
    public double BattleTime { get; set; }
}