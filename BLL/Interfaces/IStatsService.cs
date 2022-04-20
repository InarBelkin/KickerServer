using BLL.Dtos.Stats;
using BLL.Models.Stats;

namespace BLL.Interfaces;

public interface IStatsService
{
    public Task<LeaderboardWrapper> GetLeadersList();
    public Task<UserDetailsDto?> GetUserDetails(Guid userGuid);
}