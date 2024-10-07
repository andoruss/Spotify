using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Domain;

namespace Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;

public interface IProviderDbContext
{
    DbSet<Domain.Bar> Bars { get; set; }
    DbSet<Domain.Provider> Providers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
