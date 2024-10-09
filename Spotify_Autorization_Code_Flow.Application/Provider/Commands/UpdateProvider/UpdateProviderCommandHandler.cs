using AutoMapper;
using MediatR;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Application.DTOs;

namespace Spotify_Autorization_Code_Flow.Application.Provider.Commands.UpdateProvider;

public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, ProviderResponse>
{
    private readonly IProviderDbContext _providerDbContext;
    private readonly IMapper _mapper;

    public UpdateProviderCommandHandler(IProviderDbContext providerDbContext, IMapper mapper)
    {
        _providerDbContext = providerDbContext;
        _mapper = mapper;
    }

    public async Task<ProviderResponse> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = _mapper.Map<Domain.Provider>(request.Provider);
        _providerDbContext.Providers.Update(provider);

        await _providerDbContext.SaveChangesAsync(CancellationToken.None);

        var response = _mapper.Map<ProviderResponse>(provider);

        return response;
    }
}
