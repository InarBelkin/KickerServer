using DAL.Entities;

namespace BLL.Dtos.Stats;

public class LeaderboardWrapper
{
    public List<UserLeaderboardDto> Data { get; set; } = new();
}