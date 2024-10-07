using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace Spotify_Autorization_Code_Flow.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly SpotifySettings _authenticationSettings;
    private readonly HttpClient _httpClient;
    

    public SpotifyController(HttpClient httpClient, IOptions<SpotifySettings> spotifySettings)
    {
        _authenticationSettings = spotifySettings.Value;
        _httpClient = httpClient;
    }

    /// <summary>
    /// demande l'autorisation à l'utilisateur et le redirige vers une url
    /// tous les paramètres sont dans un application depuis le dashboard spotify
    /// https://developer.spotify.com/dashboard/
    /// </summary>
    /// <returns></returns>
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var clientId = _authenticationSettings.ClientId;
        var redirectUri = _authenticationSettings.RedirectUri;
        var scope = "user-read-private user-read-email"; // Spécifiez les scopes nécessaires
        var url = $"https://accounts.spotify.com/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}";

        return Redirect(url);
    }

    /// <summary>
    /// route de la redirection_uri et récupère un token qui va permettre de faire des calls à l'api
    /// </summary>
    /// <param name="code"></param>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery]  string code)
    {
        var clientId = _authenticationSettings.ClientId;
        var clientSecret = _authenticationSettings.ClientSecret;
        var redirectUri = _authenticationSettings.RedirectUri;

        var tokenUrl = "https://accounts.spotify.com/api/token";
        var client = new HttpClient();

        var requestBody = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });

        var response = await client.PostAsync(tokenUrl, requestBody);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Error retrieving access token.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic tokenResponse = JsonConvert.DeserializeObject(jsonResponse);
        var accessToken = tokenResponse.access_token;

        // Vous pouvez maintenant utiliser l'accessToken pour appeler l'API Spotify
        return Ok(new { accessToken });
    }

    // Nouvelle méthode : Utiliser le refresh token pour obtenir un nouveau jeton d'accès
    //[HttpGet("refresh-token")]
    //public async Task<IActionResult> RefreshToken()
    //{
    //    if (string.IsNullOrEmpty(_refreshToken))
    //    {
    //        return BadRequest("No refresh token found. Please authorize again.");
    //    }

    //    var clientId = _configuration["Spotify:ClientId"];
    //    var clientSecret = _configuration["Spotify:ClientSecret"];
    //    var tokenUrl = "https://accounts.spotify.com/api/token";

    //    // Construire la requête pour rafraîchir le jeton d'accès
    //    var requestBody = new FormUrlEncodedContent(new[]
    //    {
    //            new KeyValuePair<string, string>("grant_type", "refresh_token"),
    //            new KeyValuePair<string, string>("refresh_token", _refreshToken),
    //            new KeyValuePair<string, string>("client_id", clientId),
    //            new KeyValuePair<string, string>("client_secret", clientSecret)
    //        });

    //    var response = await _httpClient.PostAsync(tokenUrl, requestBody);
    //    if (!response.IsSuccessStatusCode)
    //    {
    //        return BadRequest("Error refreshing access token.");
    //    }

    //    var jsonResponse = await response.Content.ReadAsStringAsync();
    //    dynamic tokenResponse = JsonConvert.DeserializeObject(jsonResponse);
    //    _accessToken = tokenResponse.access_token;

    //    // Si le refresh token a changé (rare, mais possible), mettez-le à jour
    //    if (tokenResponse.refresh_token != null)
    //    {
    //        _refreshToken = tokenResponse.refresh_token;
    //    }

    //    return Ok(new { accessToken = _accessToken, refreshToken = _refreshToken });
    //}
}
