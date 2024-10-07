using Microsoft.AspNetCore.Mvc;
using Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBar;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Presentation.Controllers;

public class BarController : EntityController
{
    [HttpGet("/{barId}")]
    public async Task<ActionResult<BarResponse>> GetBarById([FromRoute] int barId)
    {
        var query = new GetBarQuery(barId);
        return await Mediator.Send(query);
    }
}
