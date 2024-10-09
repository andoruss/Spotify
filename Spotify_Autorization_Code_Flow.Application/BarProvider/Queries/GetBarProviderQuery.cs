using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Queries;

public record class GetBarProviderQuery(Guid BarProviderId) : IRequest<BarProviderResponse>;