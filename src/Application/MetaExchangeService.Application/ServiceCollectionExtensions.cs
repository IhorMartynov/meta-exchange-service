using MetaExchangeService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MetaExchangeService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrdersService, OrdersService>()
            .AddScoped<IAccountsService, AccountsService>();

        return services;
    }
}