using Mapster;
using MetaExchangeService.Domain.Exceptions;
using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contexts;
using MetaExchangeService.Repositories.Contracts.Repositories;
using MetaExchangeService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetaExchangeService.Repositories.Repositories;

internal sealed class OrdersRepository : IOrdersRepository
{
    private readonly OrderBookContext _context;

    public OrdersRepository(OrderBookContext context) =>
        _context = context;

    /// <inheritdoc />
    public async Task<Order?> GetAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .Include(x => x.Exchange)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.Adapt<Order>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), page, "Page number cannot be less than 1.");
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size cannot be less than 1.");

        var entities = await _context.Orders
            .Include(x => x.Exchange)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return entities.Select(x => x.Adapt<Order>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> GetAskOrdersSortedByPriceAscendingAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), page, "Page number cannot be less than 1.");
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size cannot be less than 1.");

        var entities = await _context.Orders
            .Include(x => x.Exchange)
            .Where(x => x.Type ==  OrderType.Ask)
            .OrderBy(x => x.Price)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return entities.Select(x => x.Adapt<Order>());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> GetBidOrdersSortedByPriceDescendingAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page), page, "Page number cannot be less than 1.");
        if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size cannot be less than 1.");

        var entities = await _context.Orders
            .Include(x => x.Exchange)
            .Where(x => x.Type == OrderType.Bid)
            .OrderByDescending(x => x.Price)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return entities.Select(x => x.Adapt<Order>());
    }

    /// <inheritdoc />
    public async Task<Order> CreateAsync(long exchangeId, CreateOrderModel order, CancellationToken cancellationToken)
    {
        var exchangeEntity = await _context.Exchanges.FirstOrDefaultAsync(x => x.Id == exchangeId, cancellationToken)
                     ?? throw new EntityNotFoundException(nameof(ExchangeEntity), exchangeId);

        var entity = order.Adapt<OrderEntity>();
        entity.Exchange = exchangeEntity;

        await _context.Orders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Adapt<Order>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> CreateAsync(long exchangeId, CreateOrderModel[] orders, CancellationToken cancellationToken)
    {
        var exchangeEntity = await _context.Exchanges.FirstOrDefaultAsync(x => x.Id == exchangeId, cancellationToken)
                             ?? throw new EntityNotFoundException(nameof(ExchangeEntity), exchangeId);

        if (!orders.Any()) return Enumerable.Empty<Order>();

        var entities = orders.Select(x =>
            {
                var entity = x.Adapt<OrderEntity>();
                entity.Exchange = exchangeEntity;
                return entity;
            })
            .ToArray();

        await _context.Orders.AddRangeAsync(entities, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entities.Select(x => x.Adapt<Order>());
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                     ?? throw new EntityNotFoundException(nameof(OrderEntity), id);

        _context.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}