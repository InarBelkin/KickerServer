using AutoMapper;
using BLL.Dtos.Lobby;
using BLL.Models.Stats;
using DAL.Entities;
using DAL.InMemoryEntities.Lobby;

namespace BLL.Util;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDetailsDto>();
        CreateMap<StatsOneVsOne, StatsOneVsOneM>();
        CreateMap<StatsTwoVsTwo, StatsTwoVsTwoM>();

        CreateMap<LobbyItemM, LobbyItem>().ForMember(dest => dest.Initiator, opt =>
            opt.MapFrom(d => d.Initiator.Id));
        CreateMap<LobbyUserShortInfo, LobbyUser>();
        CreateMap<LobbyTimeStampM, LobbyTimeStamp>();
        CreateMap<LobbyResultM, LobbyResult>();
    }
}