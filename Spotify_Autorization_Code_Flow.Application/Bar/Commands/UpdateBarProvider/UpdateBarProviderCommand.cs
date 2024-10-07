using MediatR;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Commands.UpdateBarProvider;

public record class UpdateBarProviderCommand(Domain.Bar Bar) : IRequest<BarWithProviderResponse>;
