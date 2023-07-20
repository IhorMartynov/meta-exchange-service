using MetaExchangeService.Repositories.Entities;
using MetaExchangeService.Repositories.Models;
using MetaExchangeService.Repositories.Properties;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using MetaExchangeService.Domain.Models;

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

        SeedOrders(binanceExchangeEntity, Encoding.UTF8.GetString(Resources.OrderBook1));
        SeedOrders(krakenExchangeEntity, Encoding.UTF8.GetString(Resources.OrderBook2));

        SaveChanges();
    }

    private void SeedOrders(ExchangeEntity exchange, string json)
    {
        var orders = JsonSerializer.Deserialize<OrdersBookImportModel>(json)!;

        var askEntities = orders.Asks.Select(x => new OrderEntity
            {
                Time = x.Order.Time,
                Exchange = exchange,
                Type = OrderType.Ask,
                Amount = x.Order.Amount,
                Price = x.Order.Price
            })
            .ToArray();
        var bidEntities = orders.Bids.Select(x => new OrderEntity
            {
                Time = x.Order.Time,
                Exchange = exchange,
                Type = OrderType.Bid,
                Amount = x.Order.Amount,
                Price = x.Order.Price
            })
            .ToArray();

        Orders.AddRange(askEntities);
        Orders.AddRange(bidEntities);
    }
}