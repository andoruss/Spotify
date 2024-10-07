using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Spotify_Autorization_Code_Flow.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class EntityController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
