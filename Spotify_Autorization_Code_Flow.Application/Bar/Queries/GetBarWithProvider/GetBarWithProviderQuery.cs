using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBarWithProvider;

public record class GetBarWithProviderQuery(int Id) : IRequest<BarWithProviderResponse>;
