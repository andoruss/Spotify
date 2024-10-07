namespace Spotify_Autorization_Code_Flow.Persistance.SpotifySettings;

public class Settings
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string RedirectUri { get; set; }
}
