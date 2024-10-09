using System.Text.Json.Serialization;

namespace Spotify_Autorization_Code_Flow.Domain;

public class Provider
{
    public Guid ProviderId { get; set; } = Guid.NewGuid();
    public string Name { get; set; }

    public Guid? BarProviderId { get; set; }

    [JsonIgnore]
    public BarProvider BarProvider { get; set; }
}
