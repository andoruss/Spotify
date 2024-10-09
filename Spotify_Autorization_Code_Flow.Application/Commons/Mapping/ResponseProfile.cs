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

        CreateMap<BarWithProviderResponse, Domain.Bar>();

        CreateMap<Domain.Provider, ProviderResponse>();
        CreateMap<ProviderResponse, Domain.Provider>();

        CreateMap<Domain.BarProvider, BarProviderResponse>();
        CreateMap<BarProviderResponse, Domain.BarProvider > ();
    }
}
