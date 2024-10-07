using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Spotify_Autorization_Code_Flow.Application.Commons.Mapping;
using System.Reflection;

namespace Spotify_Autorization_Code_Flow.Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));

        });

        //config automapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ResponseProfile());
        });

        var mapper = mapperConfig.CreateMapper();

        services.AddSingleton<IMapper>(mapper);

        return services;
    }
}
