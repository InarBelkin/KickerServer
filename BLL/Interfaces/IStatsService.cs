using BLL.Dtos.Lobby;
using BLL.Dtos.Stats;
using BLL.Models.Stats;
using DAL.Entities;

namespace BLL.Interfaces;

public interface IStatsService
{
    public Task<LeaderboardWrapper> GetLeadersList();
    public Task<UserDetailsDto?> GetUserDetails(Guid userGuid);
    Task<UserDetailsDto?> GetMyPage();

    Task<UserLeaderboardDto?> GetLeader(Guid userId);
    Task<LobbyUserShortInfo?> GetUserShortInfoPartial(Guid? userId);
}