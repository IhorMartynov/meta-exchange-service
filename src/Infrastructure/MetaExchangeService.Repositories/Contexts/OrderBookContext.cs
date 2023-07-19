using MetaExchangeService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetaExchangeService.Repositories.Contexts;

public sealed class OrderBookContext : DbContext
{
    public DbSet<ExchangeEntity> Exchanges { get; set; } = null!;
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<OrderEntity> Orders { get; set; } = null!;

    public OrderBookContext(DbContextOptions<OrderBookContext> options)
        : base(options)
    {
        if (!Database.EnsureCreated()) return;

        var binanceExchangeEntity = new ExchangeEntity {Name = "Binance"};
        var krakenExchangeEntity = new ExchangeEntity { Name = "Kraken" };
        Exchanges.Add(binanceExchangeEntity);
        Exchanges.Add(krakenExchangeEntity);

        Accounts.Add(new AccountEntity {Exchange = binanceExchangeEntity, BtcAmount = 2, EurAmount = 4000});
        Accounts.Add(new AccountEntity {Exchange = krakenExchangeEntity, BtcAmount = 3, EurAmount = 5000});

        SaveChanges();
    }
}