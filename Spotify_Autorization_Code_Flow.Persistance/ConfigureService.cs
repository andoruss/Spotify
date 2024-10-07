
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotify_Autorization_Code_Flow.Application.Commons.Interface.Context;
using Spotify_Autorization_Code_Flow.Persistance.Context;
using Spotify_Autorization_Code_Flow.Persistance.SpotifySettings;

namespace Spotify_Autorization_Code_Flow.Persistance;

public static class ConfigureService
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Chaîne de connection à la base de données
        services.AddDbContext<ProviderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProviderDbContext, ProviderDbContext>();

        return services;
    }
}
