using Spotify_Autorization_Code_Flow.Domain;

namespace Spotify_Autorization_Code_Flow.Application.DTOs;

public class ProviderResponse
{
    public Guid ProviderId { get; set; }
    public int BarId { get; set; } 

    // Jeton d'accès à l'API Spotify
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public string TokenType { get; set; }

    public int ExpiresIn { get; set; }

    public DateTime ExpiresAt { get; set; }
}
