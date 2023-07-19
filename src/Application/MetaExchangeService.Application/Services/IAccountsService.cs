using MetaExchangeService.Domain.Exceptions;
using MetaExchangeService.Domain.Models;

namespace MetaExchangeService.Application.Services;

public interface IAccountsService
{
    /// <summary>
    /// Get an exchange account information.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <returns></returns>
    Task<Account?> GetByExchangeAsync(long exchangeId, CancellationToken cancellationToken);

    /// <summary>
    /// Get all exchange accounts.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken);
}