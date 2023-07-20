using System.Runtime.CompilerServices;
using MetaExchangeService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("MetaExchangeService.Application.Tests")]
namespace MetaExchangeService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrdersService, OrdersService>()
            .AddScoped<IAccountsService, AccountsService>()
            .AddScoped<IExecutionPlanService, ExecutionPlanService>();

        return services;
    }
}