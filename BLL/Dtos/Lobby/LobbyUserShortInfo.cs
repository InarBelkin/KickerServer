using DAL.Entities;
using GeneralLibrary.Enums;

namespace BLL.Dtos.Lobby;

public class LobbyUserShortInfo
{
    public LobbyUserShortInfo()
    {
    }

    public LobbyUserShortInfo(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Elo = user.StatsOneVsOne?.ELO ?? 0;
    }

    public Guid? Id { get; set; }
    public Role Role { get; set; }
    public IsAccepted Accepted { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; }
}