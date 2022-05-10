using DAL.InMemoryEntities.Lobby;

namespace BLL.Dtos.Lobby;

public class LobbyResultM
{
    public LobbyResultM()
    {
    }

    public LobbyResultM(LobbyResult lobby)
    {
        this.IsWinnerA = lobby.IsWinnerA;
        this.CountOfGoalsLoser = lobby.CountOfGoalsLoser;
    }

    public bool? IsWinnerA { get; set; }
    public int CountOfGoalsLoser { get; set; }
}