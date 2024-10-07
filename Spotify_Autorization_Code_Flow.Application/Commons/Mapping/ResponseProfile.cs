using AutoMapper;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Commons.Mapping;

public class ResponseProfile : Profile
{
    public ResponseProfile()
    {
        CreateMap<Domain.Bar, BarResponse>();
        CreateMap<BarResponse, Domain.Bar>();
        CreateMap<Domain.Bar, BarWithProviderResponse>();
            
        CreateMap<BarWithProviderResponse, Domain.Bar>()
            .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.Provider.ProviderId));

        CreateMap<Domain.Provider, ProviderResponse>();
    }
}
