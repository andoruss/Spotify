using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Commands.AddProvider;

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, ProviderResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public CreateProviderCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<ProviderResponse> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        await _providerDbContext.Providers.AddRangeAsync(request.Provider);

        await _providerDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProviderResponse>(request.Provider);
    }
}
