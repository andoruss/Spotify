using System.Text.Json.Serialization;

namespace Spotify_Autorization_Code_Flow.Domain;

public class Provider
{
    public Guid ProviderId { get; set; } = Guid.NewGuid(); // Clé primaire pour identifier l'entité dans la base de données

    // Identifiant de l'utilisateur pour lequel ces tokens sont stockés (si applicable)
    public int BarId { get; set; }

    [JsonIgnore] // Add this attribute
    public Bar Bar { get; set; }

    // Jeton d'accès à l'API Spotify
    public string AccessToken { get; set; }

    // Jeton de rafraîchissement pour renouveler le jeton d'accès
    public string RefreshToken { get; set; }

    // Type de jeton (généralement "Bearer" pour Spotify)
    public string TokenType { get; set; }

    // Durée de vie en secondes du jeton d'accès (généralement 3600 secondes)
    public int ExpiresIn { get; set; }

    // Date et heure d'expiration du jeton d'accès (calculée à partir de ExpiresIn)
    public DateTime ExpiresAt { get; set; }


    // Méthode pour vérifier si le jeton d'accès est expiré
    public bool IsAccessTokenExpired()
    {
        return DateTime.UtcNow >= ExpiresAt;
    }
}
