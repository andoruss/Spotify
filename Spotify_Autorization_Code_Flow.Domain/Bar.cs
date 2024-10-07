namespace Spotify_Autorization_Code_Flow.Domain;

public class Bar
{
    public int BarId { get; set; } // Clé primaire pour identifier l'entité dans la base de données

    public Guid? ProviderId { get; set; }
    public Provider? Provider { get; set; }
}
