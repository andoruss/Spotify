using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.AddBarProvider;

public record class AddBarProviderCommand(Domain.BarProvider BarProvider) : IRequest<BarProviderResponse>;
