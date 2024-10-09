using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Queries.GetProvider;

public record class GetProviderQuery(Guid ProviderId) : IRequest<ProviderResponse>;
