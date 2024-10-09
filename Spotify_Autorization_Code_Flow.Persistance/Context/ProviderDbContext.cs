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
    public DbSet<BarProvider> BarProviders { get; set; }
 
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration des relations entre les entités
        modelBuilder.Entity<Bar>()
                .HasKey(b => b.BarId);

        modelBuilder.Entity<BarProvider>()
            .HasKey(bp => bp.BarProviderId);

        modelBuilder.Entity<BarProvider>()
            .HasOne(bp => bp.Bar)
            .WithMany(b => b.BarProviders)
            .HasForeignKey(bp => bp.BarId)
            .OnDelete(DeleteBehavior.Cascade); // Example: Cascade delete if a Bar is deleted

        modelBuilder.Entity<BarProvider>()
            .HasOne(bp => bp.Provider)
            .WithOne(p => p.BarProvider);

        modelBuilder.Entity<Provider>()
            .HasKey(p => p.ProviderId);

        modelBuilder.Entity<BarProvider>()
                .HasOne(bp => bp.Provider)
                .WithOne(p => p.BarProvider)
                .HasForeignKey<Provider>(p => p.BarProviderId);
    }
}
