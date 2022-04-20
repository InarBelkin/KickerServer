using System;
using AutoMapper;
using BLL.Models.Stats;
using DAL.Entities;
using Xunit;

namespace UnitTest.AutoMapper;

public class MapTest
{
    [Fact]
    public void Test1()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDetailsDto>();
            cfg.CreateMap<StatsOneVsOne, StatsOneVsOneM>();
            cfg.CreateMap<StatsTwoVsTwo, StatsTwoVsTwoM>();
        });

        var mapper = configuration.CreateMapper();

        User user = new User()
        {
            Name = "Inar", Id = Guid.Empty, StatsOneVsOne = new() {BattlesCount = 123},
            StatsTwoVsTwo = new() {ELO = 999}
        };

        var userDetail = mapper.Map<User, UserDetailsDto>(user);
    }
    
}