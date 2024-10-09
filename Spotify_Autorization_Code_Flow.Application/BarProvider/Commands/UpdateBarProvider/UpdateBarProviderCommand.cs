using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.UpdateBarProvider;

public record class UpdateBarProviderCommand(Domain.BarProvider BarProvider) : IRequest<BarProviderResponse>;