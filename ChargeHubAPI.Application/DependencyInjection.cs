using System.Reflection;
using ChargeHubAPI.Application.Interfaces;
using ChargeHubAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChargeHubAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}




