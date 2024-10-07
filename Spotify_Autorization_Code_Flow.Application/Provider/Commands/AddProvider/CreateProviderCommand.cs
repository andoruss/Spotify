using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Commands.AddProvider;

public record class CreateProviderCommand(Domain.Provider Provider) : IRequest<ProviderResponse>;
