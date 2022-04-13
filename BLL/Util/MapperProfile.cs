using AutoMapper;
using BLL.Models.Stats;
using DAL.Entities;

namespace BLL.Util;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDetailsDto>();
        CreateMap<StatsOneVsOne, StatsOneVsOneM>();
        CreateMap<StatsTwoVsTwo, StatsTwoVsTwoM>();
    }
}