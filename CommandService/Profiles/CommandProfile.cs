using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Source->Target
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();

            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatfomPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalId, options => options.MapFrom(src => src.Id));

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalId, options => options.MapFrom(x => x.PlatformId))
                .ForMember(dest => dest.Commands, options => options.Ignore());
        }
    }
}
