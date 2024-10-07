using Microsoft.EntityFrameworkCore;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Domain;

namespace Spotify_Autorization_Code_Flow.Persistance.Context;

public class ProviderDbContext : DbContext, IProviderDbContext
{
    public ProviderDbContext(DbContextOptions<ProviderDbContext> options) : base(options)
    {
    }

    public DbSet<Bar> Bars { get; set; }
    public DbSet<Provider> Providers { get; set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration des relations entre les entités
        modelBuilder.Entity<Bar>()
            .HasOne(b => b.Provider)
            .WithOne(p => p.Bar)
            .HasForeignKey<Provider>(p => p.BarId)
            .OnDelete(DeleteBehavior.Cascade); // Supprime le Provider si le Bar correspondant est supprimé

        // Configuration des clés étrangères, s'il y a lieu
        modelBuilder.Entity<Provider>()
            .HasKey(p => p.BarId);
    }
}
