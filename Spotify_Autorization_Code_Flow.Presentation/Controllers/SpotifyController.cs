using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spotify_Autorization_Code_Flow.Application.Bar.Commands.UpdateBarProvider;
using Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBar;
using Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBarWithProvider;
using Spotify_Autorization_Code_Flow.Application.DTOs;
using Spotify_Autorization_Code_Flow.Application.Provider.Commands.AddProvider;
using Spotify_Autorization_Code_Flow.Domain;
using Spotify_Autorization_Code_Flow.Persistance.SpotifySettings;

namespace Spotify_Autorization_Code_Flow.Presentation.Controllers;


public class SpotifyController : EntityController
{
    private readonly Settings _authenticationSettings;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public SpotifyController(HttpClient httpClient, IOptions<Settings> spotifySettings, IMapper mapper)
    {
        _authenticationSettings = spotifySettings.Value;
        _httpClient = httpClient;
        _mapper = mapper;
    }

    /// <summary>
    /// demande l'autorisation à l'utilisateur et le redirige vers une url
    /// tous les paramètres sont dans un application depuis le dashboard spotify
    /// https://developer.spotify.com/dashboard/
    /// </summary>
    /// <returns></returns>
    [HttpGet("authorize/{barId}")]
    public async Task<ActionResult> Authorize([FromRoute] int barId)
    {
        var queryBar = new GetBarQuery(barId);
        var findBar = await Mediator.Send(queryBar);

        if(findBar == null)
        {
            return NotFound("Ce bar n'existe pas");
        }

        //paramètres de l'application spotify
        var clientId = _authenticationSettings.ClientId;
        var redirectUri = _authenticationSettings.RedirectUri;
        var scope = "user-read-private playlist-read-private user-read-email user-library-read"; 


        var url = $"https://accounts.spotify.com/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}&state={barId}";

        return Redirect(url);
    }

    /// <summary>
    /// route de la redirection_uri et récupère un token qui va permettre de faire des calls à l'api
    /// </summary>
    /// <param name="code"></param>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] int state)
    {
        var clientId = _authenticationSettings.ClientId;
        var clientSecret = _authenticationSettings.ClientSecret;
        var redirectUri = _authenticationSettings.RedirectUri;

        
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", redirectUri),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });

        //à mettre dans la couche external
        //url spotify pour récupérer le token
        var tokenUrl = "https://accounts.spotify.com/api/token";
        var response = await _httpClient.PostAsync(tokenUrl, requestBody);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Error retrieving access token.");
        }

        //Désérialiser la réponse
        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic tokenResponse = JsonConvert.DeserializeObject(jsonResponse);

        //données à enregistrer
        var accessToken = tokenResponse.access_token;
        var refreshToken = tokenResponse.refresh_token;
        var expiresIn = tokenResponse.expires_in;

        // Calculer la date d'expiration du jeton d'accès
        var expiresAt = DateTime.UtcNow.AddSeconds((int)expiresIn);


        var queryBar = new GetBarWithProviderQuery(state);
        var bar = await Mediator.Send(queryBar);

        if (bar == null)
        {
            return NotFound("Ce bar n'existe pas");
        }

        var mappingBar = _mapper.Map<Bar>(bar);

        // Enregistrer ou mettre à jour les tokens dans la base de données
        if (mappingBar.ProviderId == null)
        {
            var provider = new Provider
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                ExpiresAt = expiresAt
            };

            mappingBar.ProviderId = provider.ProviderId;
            mappingBar.Provider = provider;
        }
        else
        {
            mappingBar.Provider.AccessToken = accessToken;
            mappingBar.Provider.RefreshToken = refreshToken;
            mappingBar.Provider.ExpiresIn = expiresIn;
            mappingBar.Provider.ExpiresAt = expiresAt;
        }    

        var queryUpdateBar = new UpdateBarProviderCommand(mappingBar);
        var barUpdate = await Mediator.Send(queryUpdateBar);

        return Ok(barUpdate);
    }



    // Nouvelle méthode : Utiliser le refresh token pour obtenir un nouveau jeton d'accès
    [HttpGet("refresh-token/{refreshToken}/bar/{barId}")]
    public async Task<IActionResult> RefreshToken([FromRoute] string refreshToken, [FromRoute] int barId)
    {
        var clientId = _authenticationSettings.ClientId;
        var clientSecret = _authenticationSettings.ClientSecret;
        var tokenUrl = "https://accounts.spotify.com/api/token";

        // Construire la requête pour rafraîchir le jeton d'accès
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });
      
        //var cherhcer le bar en base
        var queryBarWithProvide = new GetBarWithProviderQuery(barId);
        var repsonseBar = await Mediator.Send(queryBarWithProvide);
        if (repsonseBar == null)
        {
            return NotFound("Ce bar n'existe pas");
        }

        //mapper le bar et vérifie si le provider est null
        var bar = _mapper.Map<Bar>(repsonseBar);
        if (bar.Provider == null)
        {
            return NotFound("Aucun provider n'est dans ce bar");
        }

        //va chercher un nouveau token
        var response = await _httpClient.PostAsync(tokenUrl, requestBody);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Error refreshing access token.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic tokenResponse = JsonConvert.DeserializeObject(jsonResponse);
        var expiresAt = DateTime.UtcNow.AddSeconds((int)tokenResponse.expires_in);

        bar.Provider.AccessToken = tokenResponse.access_token;
        bar.Provider.ExpiresIn = tokenResponse.expires_in;
        bar.Provider.ExpiresAt = expiresAt;

        // Si le refresh token a changé (rare, mais possible), mettez-le à jour
        if (tokenResponse.refresh_token != null)
        {
            bar.Provider.RefreshToken = tokenResponse.refresh_token;
        }

        var queryUpdateBar = new UpdateBarProviderCommand(bar);
        var barUpdate = await Mediator.Send(queryUpdateBar);

        return Ok(barUpdate);
    }
}