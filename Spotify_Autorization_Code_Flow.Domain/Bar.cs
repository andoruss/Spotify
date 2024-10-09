using System.Text.Json.Serialization;

namespace Spotify_Autorization_Code_Flow.Domain;

public class Bar
{
    public int BarId { get; set; } // Clé primaire pour identifier l'entité dans la base de données

    [JsonIgnore]
    public IEnumerable<BarProvider> BarProviders { get; set; } = [];
}
