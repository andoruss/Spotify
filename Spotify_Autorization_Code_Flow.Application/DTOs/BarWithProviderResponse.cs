using Spotify_Autorization_Code_Flow.Domain;

namespace Spotify_Autorization_Code_Flow.Application.DTOs;

public class BarWithProviderResponse
{
    public int BarId { get; set; }
    public IEnumerable<Domain.BarProvider> BarProviders { get; set; }
}
