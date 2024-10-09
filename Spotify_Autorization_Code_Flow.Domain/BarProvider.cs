namespace Spotify_Autorization_Code_Flow.Domain;

public class BarProvider
{
    public Guid BarProviderId { get; set; } = Guid.NewGuid();

    public int BarId { get; set; }
    public Bar Bar { get; set; }

    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; }

    //info token
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpiresAt { get; set; }

    public bool IsAccessTokenExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }
}
