namespace Spotify_Autorization_Code_Flow.Application.DTOs;

public class BarProviderResponse
{
    public Guid BarProviderId { get; set; }
    public int BarId { get; set; }
    public Guid ProviderId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiresAt { get; set; }
}
