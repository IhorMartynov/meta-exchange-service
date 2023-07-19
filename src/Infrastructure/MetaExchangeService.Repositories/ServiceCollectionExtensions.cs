using MetaExchangeService.Repositories.Contexts;
using MetaExchangeService.Repositories.Contracts.Repositories;
using MetaExchangeService.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MetaExchangeService.Repositories
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<OrderBookContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IOrdersRepository, OrdersRepository>()
                .AddScoped<IAccountsRepository, AccountsRepository>();

            return services;
        }
    }
}
