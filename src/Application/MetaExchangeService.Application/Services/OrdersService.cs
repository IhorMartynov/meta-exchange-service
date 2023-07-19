using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contracts.Repositories;

namespace MetaExchangeService.Application.Services;

internal sealed class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;

    public OrdersService(IOrdersRepository ordersRepository) =>
        _ordersRepository = ordersRepository;

    /// <inheritdoc />
    public Task<Order?> GetAsync(long id, CancellationToken cancellationToken) =>
        _ordersRepository.GetAsync(id, cancellationToken);

    /// <inheritdoc />
    public Task<IEnumerable<Order>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken) =>
        _ordersRepository.GetAllAsync(page, pageSize, cancellationToken);

    /// <inheritdoc />
    public Task<Order> CreateAsync(long exchangeId, CreateOrderModel order, CancellationToken cancellationToken) =>
        _ordersRepository.CreateAsync(exchangeId, order, cancellationToken);

    /// <inheritdoc />
    public Task<IEnumerable<Order>> CreateAsync(long exchangeId, CreateOrderModel[] orders, CancellationToken cancellationToken) =>
        _ordersRepository.CreateAsync(exchangeId, orders, cancellationToken);

    /// <inheritdoc />
    public Task DeleteAsync(long id, CancellationToken cancellationToken) =>
        _ordersRepository.DeleteAsync(id, cancellationToken);
}