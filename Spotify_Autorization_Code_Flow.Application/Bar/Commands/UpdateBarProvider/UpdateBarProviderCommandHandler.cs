using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Bar.Commands.UpdateBarProvider;

public class UpdateBarProviderCommandHandler : IRequestHandler<UpdateBarProviderCommand, BarWithProviderResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public UpdateBarProviderCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<BarWithProviderResponse> Handle(UpdateBarProviderCommand request, CancellationToken cancellationToken)
    {
        var bar = _mapper.Map<Domain.Bar>(request.Bar);

        _providerDbContext.Bars.UpdateRange(bar);

        await _providerDbContext.SaveChangesAsync(CancellationToken.None);

        var response = _mapper.Map<BarWithProviderResponse>(bar);

        return response;
    }
}
