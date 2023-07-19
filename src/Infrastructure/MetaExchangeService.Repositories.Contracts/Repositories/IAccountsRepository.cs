using MetaExchangeService.Domain.Exceptions;
using MetaExchangeService.Domain.Models;

namespace MetaExchangeService.Repositories.Contracts.Repositories;

public interface IAccountsRepository
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