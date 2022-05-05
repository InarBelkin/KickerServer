using AutoMapper;
using BLL.Dtos.Lobby;
using BLL.Dtos.Stats;
using BLL.Interfaces;
using BLL.Models.Stats;
using DAL.Entities;
using GeneralLibrary.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public class StatsService : ServiceBasePg, IStatsService
{
    private readonly IMapper _mapper;

    protected ILobbyService LobbyService => _lobbyService ??= HttpContext.RequestServices.GetService<ILobbyService>()!;

    private ILobbyService? _lobbyService;

    protected IAuthService AuthService => _authService ??= HttpContext.RequestServices.GetService<IAuthService>()!;

    private IAuthService? _authService;

    public StatsService(IHttpContextAccessor accessor, IMapper mapper) : base(accessor)
    {
        _mapper = mapper;
    }

    public async Task<LeaderboardWrapper> GetLeadersList()
    {
        var lobbyHash = await LobbyService.GetAllPlayingUsers();
        var ret = await Db.Users.Include(u => u.StatsOneVsOne).Include(u => u.StatsTwoVsTwo)
            .Select(u => new UserLeaderboardDto(u, lobbyHash))
            .ToListAsync();


        return new LeaderboardWrapper()
        {
            Data = ret
        };
    }

    public async Task<UserLeaderboardDto?> GetLeader(Guid userId)
    {
        var lobbyHash = await LobbyService.GetAllPlayingUsers();
        var user = await Db.Users.Where(u => u.Id == userId).Include(u => u.StatsOneVsOne)
            .Include(u => u.StatsTwoVsTwo)
            .FirstOrDefaultAsync();
        if (user == null) return null;
        return new UserLeaderboardDto(user, lobbyHash);
    }


    public async Task<LobbyUserShortInfo?> GetUserShortInfoPartial(Guid? userId)
    {
        if (userId == null) return null;
        return await Db.Users.Include(u => u.StatsOneVsOne).Where(u => u.Id == userId)
            .Select(u => new LobbyUserShortInfo() {Id = u.Id, Name = u.Name, Elo = u.StatsOneVsOne!.ELO})
            .FirstOrDefaultAsync();
    }

    public async Task<UserDetailsDto?> GetUserDetails(Guid userGuid)
    {
        var user = await Db.Users.Where(u => u.Id == userGuid).Include(u => u.StatsOneVsOne)
            .Include(u => u.StatsTwoVsTwo)
            .FirstOrDefaultAsync();
        if (user == null) return null;

        var userDto = _mapper.Map<User, UserDetailsDto>(user);
        var myId = AuthService.GetUserClaims()?.Id;
        userDto.IsMe = myId != null && myId == userDto.Id;

        return userDto;
    }

    public async Task<UserDetailsDto?> GetMyPage()
    {
        var userId = AuthService.GetUserClaims()?.Id;
        if (userId == null) return null;
        var user = await GetUserDetails(userId.Value);
        return user;
    }
}