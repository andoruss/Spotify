using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Commands.AddBarProvider;

public class AddBarProviderCommandHandler : IRequestHandler<AddBarProviderCommand, BarProviderResponse>
{
    private readonly IProviderDbContext _barProviderDbContext;
    private readonly IMapper _mapper;

    public AddBarProviderCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _barProviderDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarProviderResponse> Handle(AddBarProviderCommand request, CancellationToken cancellationToken)
    {
        await _barProviderDbContext.BarProviders.AddRangeAsync(request.BarProvider);

        await _barProviderDbContext.SaveChangesAsync(CancellationToken.None);

        return _mapper.Map<BarProviderResponse>(request.BarProvider);
    }
}
