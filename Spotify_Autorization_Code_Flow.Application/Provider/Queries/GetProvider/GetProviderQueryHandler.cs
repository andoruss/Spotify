using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Queries.GetProvider;

public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, ProviderResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public GetProviderQueryHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }
    public async Task<ProviderResponse> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await _providerDbContext.Providers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProviderId == request.ProviderId);

        return _mapper.Map<ProviderResponse>(result);
    }
}
