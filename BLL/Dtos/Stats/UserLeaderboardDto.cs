using DAL.Entities;
using GeneralLibrary.Enums;

namespace BLL.Dtos.Stats;

public class UserLeaderboardDto
{
    public UserLeaderboardDto()
    {
    }

    public UserLeaderboardDto(User user, Dictionary<Guid, UserStatus> usersStatuses)
    {
        Id = user.Id;
        Name = user.Name;
        Elo = user.StatsOneVsOne.ELO;
        CountOfBattles = user.StatsOneVsOne.BattlesCount + user.StatsTwoVsTwo.BattlesCountInAttack +
                         user.StatsTwoVsTwo.BattlesCountInDefense;
        WinsCount = user.StatsOneVsOne.WinsCount + user.StatsTwoVsTwo.WinsCountInAttack +
                    user.StatsTwoVsTwo.BattlesCountInDefense;

        this.Status = usersStatuses.ContainsKey(Id) ? usersStatuses[Id] : UserStatus.Offline;
    }


    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Elo { get; set; }
    public int CountOfBattles { get; set; }
    public double WinsCount { get; set; }
    public int Cups { get; set; }
    public UserStatus Status { get; set; }
}