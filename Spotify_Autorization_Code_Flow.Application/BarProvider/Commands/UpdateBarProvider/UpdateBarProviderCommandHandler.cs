using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.UpdateBarProvider;

public class UpdateBarProviderCommandHandler : IRequestHandler<UpdateBarProviderCommand, BarProviderResponse>
{
    private readonly IProviderDbContext _barProviderDbContext;
    private readonly IMapper _mapper;

    public UpdateBarProviderCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _barProviderDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarProviderResponse> Handle(UpdateBarProviderCommand request, CancellationToken cancellationToken)
    {
        var barProvider = _mapper.Map<Domain.BarProvider>(request.BarProvider);
        _barProviderDbContext.BarProviders.UpdateRange(barProvider);

        await _barProviderDbContext.SaveChangesAsync(CancellationToken.None);

        var response = _mapper.Map<BarProviderResponse>(request.BarProvider);

        return response;
    }
}
