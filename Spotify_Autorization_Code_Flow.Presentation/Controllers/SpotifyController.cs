using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spotify_Autorization_Code_Flow.Application.Bar.Commands.UpdateBarProvider;
using Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBar;
using Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBarWithProvider;
using Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.AddBarProvider;
using Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.UpdateBarProvider;
using Spotify_Autorization_Code_Flow.Application.BarProvider.Queries;
using Spotify_Autorization_Code_Flow.Application.Commons.Models;
using Spotify_Autorization_Code_Flow.Application.DTOs;
using Spotify_Autorization_Code_Flow.Application.Provider.Commands.AddProvider;
using Spotify_Autorization_Code_Flow.Application.Provider.Commands.UpdateProvider;
using Spotify_Autorization_Code_Flow.Application.Provider.Queries.GetProvider;
using Spotify_Autorization_Code_Flow.Domain;
using Spotify_Autorization_Code_Flow.Persistance.SpotifySettings;
using System.Net;

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
    [HttpGet("/bar/{barId}/provider/{providerId}/authorize/")]
    public async Task<ActionResult> Authorize([FromRoute] int barId, [FromRoute] Guid providerId)
    {
        var queryBar = new GetBarQuery(barId);
        var findBar = await Mediator.Send(queryBar);
        if (findBar == null)
        {
            return NotFound("Ce bar n'existe pas");
        }

        var queryProvider = new GetProviderQuery(providerId);
        var findProvider = await Mediator.Send(queryProvider);
        if (findProvider == null)
        {
            return NotFound("Ce provider n'existe pas");
        }

        //paramètres de l'application spotify
        var clientId = _authenticationSettings.ClientId;
        var redirectUri = _authenticationSettings.RedirectUri;
        var scope = "user-read-private playlist-read-private user-read-email user-library-read";

        var spotifyState = new SpotifyState
        {
            BarId = barId,
            ProviderId = providerId
        };
        var state = JsonConvert.SerializeObject(spotifyState);
        var encodedState = WebUtility.UrlEncode(state);

        var url = $"https://accounts.spotify.com/authorize?response_type=code&show_dialog=true&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}&state={encodedState}";

        return Redirect(url);
    }

    /// <summary>
    /// route de la redirection_uri et récupère un token qui va permettre de faire des calls à l'api
    /// </summary>
    /// <param name="code"></param>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        var stateObject = JsonConvert.DeserializeObject<SpotifyState>(state);

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

        //récupère le provider
        var queryProvider = new GetProviderQuery(stateObject.ProviderId);
        var handlerProvider = await Mediator.Send(queryProvider);
        var provider = _mapper.Map<Provider>(handlerProvider);

        if (provider.BarProviderId == null)
        {
            var barProvider = new BarProvider
                {
                    BarId = stateObject.BarId,
                    ProviderId = stateObject.ProviderId,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenType = "Bearer",
                    ExpiresIn = expiresIn,
                    ExpiresAt = expiresAt
                };

            provider.BarProviderId = barProvider.BarProviderId;

            //ajoute le barProvider en base
            var queryAddProvider = new AddBarProviderCommand(barProvider);
            var responseAdd = await Mediator.Send(queryAddProvider);

            //ajout l'id dans le provider
            var queryUpdateProvider = new UpdateProviderCommand(provider);
            var responseUpdateProvider = await Mediator.Send(queryUpdateProvider);
        }
        else
        {
            var queryBarProvider = new GetBarProviderQuery((Guid)provider.BarProviderId);
            var handlerBarProvider = await Mediator.Send(queryBarProvider);

            var barProvider = _mapper.Map<BarProvider>(handlerBarProvider);
            barProvider.AccessToken = accessToken;
            barProvider.RefreshToken = refreshToken;
            barProvider.ExpiresIn = expiresIn;
            barProvider.ExpiresAt = expiresAt;

            var queryUpdateBarProvider = new UpdateBarProviderCommand(barProvider);
            await Mediator.Send(queryUpdateBarProvider);
        }

        http://localhost:5219/bar/1/provider/d99b44a2-0abd-482f-b3a9-13252ef82f9f/authorize

        return Ok(provider);
    }



    // Nouvelle méthode : Utiliser le refresh token pour obtenir un nouveau jeton d'accès
    [HttpGet("refresh-token/{providerId}")]
    public async Task<IActionResult> RefreshToken([FromRoute] Guid providerId)
    {
        // Récupérer le refresh token du provider
        var queryBarProvider = new GetBarProviderQuery(providerId);
        var handlerBarProvider = await Mediator.Send(queryBarProvider);
        var barProvider = _mapper.Map<BarProvider>(handlerBarProvider);

        var clientId = _authenticationSettings.ClientId;
        var clientSecret = _authenticationSettings.ClientSecret;
        var tokenUrl = "https://accounts.spotify.com/api/token";

        // Construire la requête pour rafraîchir le jeton d'accès
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", barProvider.RefreshToken),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret)
        });


        //va chercher un nouveau token
        var response = await _httpClient.PostAsync(tokenUrl, requestBody);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Error refreshing access token.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic tokenResponse = JsonConvert.DeserializeObject(jsonResponse);
        var expiresAt = DateTime.UtcNow.AddSeconds((int)tokenResponse.expires_in);

        barProvider.AccessToken = tokenResponse.access_token;
        barProvider.ExpiresIn = tokenResponse.expires_in;
        barProvider.ExpiresAt = expiresAt;

        // Si le refresh token a changé (rare, mais possible), mettez-le à jour
        if (tokenResponse.refresh_token != null)
        {
            barProvider.RefreshToken = tokenResponse.refresh_token;
        }

        var queryUpdateBarProvider = new UpdateBarProviderCommand(barProvider);
        var barProviderUpdate = await Mediator.Send(queryUpdateBarProvider);

        return Ok(barProviderUpdate);
    }
}