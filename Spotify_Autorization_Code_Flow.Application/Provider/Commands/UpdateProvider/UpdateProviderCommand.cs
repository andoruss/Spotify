using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Commands.UpdateProvider;

public record class UpdateProviderCommand(Domain.Provider Provider) : IRequest<ProviderResponse>;
