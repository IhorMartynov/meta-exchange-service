using Alba;
using Testcontainers.MsSql;

namespace MetaExchangeService.WebApi.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected MsSqlContainer DbContainer = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        DbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        DbContainer.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        DbContainer.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        DbContainer.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    protected Task<IAlbaHost> CreateAlbaHost() =>
        AlbaHost.For<Program>(builder =>
        {
            builder.UseSetting("ConnectionStrings:ExchangeDb", DbContainer.GetConnectionString());
        }, Array.Empty<IAlbaExtension>());
}