using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Queries.GetBar;

public class GetBarQueryHandler : IRequestHandler<GetBarQuery, BarResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public GetBarQueryHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarResponse?> Handle(GetBarQuery request, CancellationToken cancellationToken)
    {
        var result = await _providerDbContext.Bars.Where(b => b.BarId == request.Id).AsNoTracking().FirstOrDefaultAsync(CancellationToken.None);

        return result == null ? null : _mapper.Map<BarResponse>(result);
    }
}
