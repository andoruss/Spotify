namespace Spotify_Autorization_Code_Flow.Application.DTOs;

public class BarWithProviderResponse
{
    public int BarId { get; set; } // Clé primaire pour identifier l'entité dans la base de données

    public Guid? ProviderId { get; set; }
    public Domain.Provider? Provider { get; set; }
}
