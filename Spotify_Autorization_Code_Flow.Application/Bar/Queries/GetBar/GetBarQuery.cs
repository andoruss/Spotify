using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBar;

public record class GetBarQuery(int Id) : IRequest<BarResponse>;
