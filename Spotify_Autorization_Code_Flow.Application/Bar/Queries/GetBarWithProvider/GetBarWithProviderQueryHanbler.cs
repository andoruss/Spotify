using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBarWithProvider;

public class GetBarWithProviderQueryHanbler : IRequestHandler<GetBarWithProviderQuery, BarWithProviderResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public GetBarWithProviderQueryHanbler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarWithProviderResponse?> Handle(GetBarWithProviderQuery request, CancellationToken cancellationToken)
    {
        var result = await _providerDbContext.Bars
            .Include(b => b.BarProviders)
            .Where(b => b.BarId == request.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(CancellationToken.None);

       return result == null ? null : _mapper.Map<BarWithProviderResponse>(result);
    }
}
