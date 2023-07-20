using MetaExchangeService.Domain.Exceptions;
using MetaExchangeService.Domain.Models;

namespace MetaExchangeService.Repositories.Contracts.Repositories
{
    public interface IOrdersRepository
    {
        /// <summary>
        /// Get an order by its identifier.
        /// </summary>
        /// <param name="id">Order identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An order object or null if an order is not found.</returns>
        Task<Order?> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Get orders by page.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Get an ASK orders page sorted by price ascending.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetAskOrdersSortedByPriceAscendingAsync(int page, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Get an BID orders page sorted by price descending.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetBidOrdersSortedByPriceDescendingAsync(int page, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Create an order.
        /// </summary>
        /// <param name="exchangeId">Exchange identifier.</param>
        /// <param name="order">Order details.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task<Order> CreateAsync(long exchangeId, CreateOrderModel order, CancellationToken cancellationToken);

        /// <summary>
        /// Create orders.
        /// </summary>
        /// <param name="exchangeId">Exchange identifier.</param>
        /// <param name="orders">An array of order details.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task<IEnumerable<Order>> CreateAsync(long exchangeId, CreateOrderModel[] orders, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an order.
        /// </summary>
        /// <param name="id">Order identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <returns></returns>
        Task DeleteAsync(long id, CancellationToken cancellationToken);
    }
}
