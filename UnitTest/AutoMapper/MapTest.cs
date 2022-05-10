using System;
using AutoMapper;
using BLL.Dtos.Lobby;
using BLL.Models.Stats;
using BLL.Util;
using DAL.Entities;
using DAL.InMemoryEntities.Lobby;
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

    [Fact]
    public void StupidTest2()
    {
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });
        var mapper = configuration.CreateMapper();

        var lobbyM = new LobbyItemM()
        {
            SideA = {new LobbyUserShortInfo()},
            SideB = {new LobbyUserShortInfo()},
        };
        var lobby = mapper.Map<LobbyItem>(lobbyM);
    }
}