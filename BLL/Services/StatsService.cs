using AutoMapper;
using BLL.Dtos.Lobby;
using BLL.Dtos.Stats;
using BLL.Interfaces;
using BLL.Models.Stats;
using DAL.Entities;
using GeneralLibrary.Enums;
using GeneralLibrary.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public sealed class StatsService : ServiceBasePg, IStatsService
{
    protected ILobbyUtilService LobbyUtilService { get; }

    private readonly IMapper _mapper;

    protected ILobbyService LobbyService => _lobbyService ??= HttpContext.RequestServices.GetService<ILobbyService>()!;

    private ILobbyService? _lobbyService;

    protected IAuthService AuthService => _authService ??= HttpContext.RequestServices.GetService<IAuthService>()!;

    private IAuthService? _authService;


    public StatsService(IHttpContextAccessor accessor, IMapper mapper, ILobbyUtilService lobbyUtilService) :
        base(accessor)
    {
        LobbyUtilService = lobbyUtilService;
        _mapper = mapper;
    }

    public async Task<LeaderboardWrapper> GetLeadersList()
    {
        var userStatuses = await LobbyUtilService.GetUserStatuses();

        var ret = await Db.Users.Include(u => u.StatsOneVsOne).Include(u => u.StatsTwoVsTwo)
            .Select(u => new UserLeaderboardDto(u, userStatuses))
            .ToListAsync();


        return new LeaderboardWrapper()
        {
            Data = ret
        };
    }

    public async Task<UserLeaderboardDto?> GetLeader(Guid userId)
    {
        var userStatuses = await LobbyUtilService.GetUserStatuses();
        var user = await Db.Users.Where(u => u.Id == userId).Include(u => u.StatsOneVsOne)
            .Include(u => u.StatsTwoVsTwo)
            .FirstOrDefaultAsync();
        if (user == null) return null;
        return new UserLeaderboardDto(user, userStatuses);
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

    public async Task ApplyLobbyStats(LobbyItemM lobbyItemM)
    {
        var isAWon = lobbyItemM.Result.IsWinnerA!.Value;
        var usersIdsA = lobbyItemM.SideA.Select(u => u.Id!.Value);
        var usersIdsB = lobbyItemM.SideB.Select(u => u.Id!.Value);

        var request = Db.Users.Include(u => u.StatsOneVsOne)
            .Include(u => u.StatsTwoVsTwo);

        var usersA = await request.Where(u => usersIdsA.Contains(u.Id)).ToListAsync();
        var usersB = await request.Where(u => usersIdsB.Contains(u.Id)).ToListAsync();

        for (int i = 0; i < usersA.Count; i++)
        {
            usersA[i].StatsOneVsOne!.ELO =
                Mathematics.CountNewElo(usersA[i].StatsOneVsOne!.ELO, usersB[0].StatsOneVsOne!.ELO, isAWon);
            usersB[i].StatsOneVsOne!.ELO =
                Mathematics.CountNewElo(usersB[i].StatsOneVsOne!.ELO, usersA[0].StatsOneVsOne!.ELO, !isAWon);
        }
        
        

        await Db.SaveChangesAsync();
    }
}