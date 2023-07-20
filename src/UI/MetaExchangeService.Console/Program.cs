using System.Reflection;
using System.Text.Json;
using MetaExchangeService.Application;
using MetaExchangeService.Application.Services;
using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(config =>
    {
        config.Sources.Clear();
        config.AddJsonFile("appsettings.json");
        config.AddCommandLine(Environment.GetCommandLineArgs());
    })
    .ConfigureServices((context, services) =>
    {
        services.AddRepositories(context.Configuration.GetConnectionString("ExchangeDb")!)
            .AddApplicationServices();
    })
    .ConfigureLogging((_, config) =>
    {
        config.SetMinimumLevel(LogLevel.Critical);
    })
    .Build();

var configuration = host.Services.GetRequiredService<IConfiguration>();

if (string.IsNullOrEmpty(configuration["btc-amount"])
    || string.IsNullOrEmpty(configuration["operation-type"]))
{
    Console.WriteLine(@$"The application finds the best execution plan.
USAGE:
{Assembly.GetExecutingAssembly().GetName().Name} btc-amount=<amount> operation-type=<buy or sell>");
}

if (!int.TryParse(configuration["btc-amount"], out var amount)) return;

var executionPlanService = host.Services.GetRequiredService<IExecutionPlanService>();

var executionPlan = configuration["operation-type"]?.ToUpper() switch
{
    "BUY" => await executionPlanService.GetBestBuyingPlanAsync(amount, CancellationToken.None),
    "SELL" => await executionPlanService.GetBestSellingPlanAsync(amount, CancellationToken.None),
    _ => Enumerable.Empty<ExecutionPlanItem>()
};

Console.WriteLine("Best execution plan:");
Console.WriteLine(JsonSerializer.Serialize(executionPlan, new JsonSerializerOptions {WriteIndented = true}));
