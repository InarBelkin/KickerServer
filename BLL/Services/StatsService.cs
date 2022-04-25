using AutoMapper;
using BLL.Dtos.Stats;
using BLL.Interfaces;
using BLL.Models.Stats;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Services;

public class StatsService : ServiceBasePg, IStatsService
{
    private readonly IMapper _mapper;

    protected IAuthService AuthService => _authService ??= HttpContext.RequestServices.GetService<IAuthService>()!;

    private IAuthService? _authService;

    public StatsService(IHttpContextAccessor accessor, IMapper mapper) : base(accessor)
    {
        _mapper = mapper;
    }

    public async Task<LeaderboardWrapper> GetLeadersList()
    {
        var ret = await Db.Users
            .Select(u => new UserLeaderboardDto()
            {
                Id = u.Id, Name = u.Name, Elo = u.StatsOneVsOne.ELO,
                CountOfBattles = u.StatsOneVsOne.BattlesCount + u.StatsTwoVsTwo.BattlesCountInAttack +
                                 u.StatsTwoVsTwo.BattlesCountInDefense,
                WinsCount = u.StatsOneVsOne.WinsCount + u.StatsTwoVsTwo.WinsCountInAttack +
                            u.StatsTwoVsTwo.BattlesCountInDefense
            }).ToListAsync();


        return new LeaderboardWrapper()
        {
            Data = ret
        };
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