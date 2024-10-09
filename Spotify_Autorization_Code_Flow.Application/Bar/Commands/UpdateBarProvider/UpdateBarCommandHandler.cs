using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Commands.UpdateBarProvider;

public class UpdateBarCommandHandler : IRequestHandler<UpdateBarCommand, BarWithProviderResponse>
{
    private readonly IProviderDbContext _barDbContext;
    private readonly IMapper _mapper;

    public UpdateBarCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _barDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarWithProviderResponse> Handle(UpdateBarCommand request, CancellationToken cancellationToken)
    {
        var bar = _mapper.Map<Domain.Bar>(request.Bar);

        _barDbContext.Bars.UpdateRange(bar);

        await _barDbContext.SaveChangesAsync(CancellationToken.None);

        var response = _mapper.Map<BarWithProviderResponse>(bar);

        return response;
    }
}
