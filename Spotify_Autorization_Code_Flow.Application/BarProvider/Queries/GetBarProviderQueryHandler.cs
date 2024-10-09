using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.BarProvider.Queries;

internal class GetBarProviderQueryHandler : IRequestHandler<GetBarProviderQuery, BarProviderResponse>
{
    private readonly IProviderDbContext _barProviderDbContext;
    private readonly IMapper _mapper;

    public GetBarProviderQueryHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _barProviderDbContext = providerDbContext;
        _mapper = mapper;
    }
    public async Task<BarProviderResponse> Handle(GetBarProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await _barProviderDbContext
            .BarProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BarProviderId == request.BarProviderId);

        return _mapper.Map<BarProviderResponse>(result);
    }
}
