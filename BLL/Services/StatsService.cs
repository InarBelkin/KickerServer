using AutoMapper;
using BLL.Dtos.Stats;
using BLL.Interfaces;
using BLL.Models.Stats;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class StatsService : ServiceBasePg, IStatsService
{
    private readonly IMapper _mapper;

    public StatsService(IHttpContextAccessor accessor, IMapper mapper) : base(accessor)
    {
        _mapper = mapper;
    }

    public async Task<List<UserLeaderboardDto>> GetLeadersList()
    {
        var request = Db.Users
            .Select(u => new UserLeaderboardDto()
            {
                Id = u.Id, Name = u.Name, Elo = u.StatsOneVsOne.ELO,
                CountOfBattles = u.StatsOneVsOne.BattlesCount + u.StatsTwoVsTwo.BattlesCountInAttack +
                                 u.StatsTwoVsTwo.BattlesCountInDefense,
                WinsCount = u.StatsOneVsOne.WinsCount + u.StatsTwoVsTwo.WinsCountInAttack +
                            u.StatsTwoVsTwo.BattlesCountInDefense
            });

        var query = request.ToQueryString();
        var ret = await request.ToListAsync();
        return ret;
    }

    public async Task<UserDetailsDto?> GetUserDetails(Guid userGuid)
    {
        var user = await Db.Users.Where(u => u.Id == userGuid).Include(u => u.StatsOneVsOne)
            .Include(u => u.StatsTwoVsTwo)
            .FirstOrDefaultAsync();

        return user == null ? null : _mapper.Map<User, UserDetailsDto>(user);
    }
}