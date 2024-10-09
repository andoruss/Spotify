using Spotify_Autorization_Code_Flow.Domain;

namespace Spotify_Autorization_Code_Flow.Application.DTOs;

public class ProviderResponse
{
    public Guid ProviderId { get; set; }
    public string Name { get; set; }
    public Guid? BarProviderId { get; set; }
}
