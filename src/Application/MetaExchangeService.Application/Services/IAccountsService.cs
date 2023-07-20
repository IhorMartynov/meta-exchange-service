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

    /// <summary>
    /// Create new account.
    /// </summary>
    /// <param name="account">Account details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Account> CreateAsync(CreateAccountModel account, CancellationToken cancellationToken);

    /// <summary>
    /// Update an account.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="account">Account details.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <returns></returns>
    Task UpdateAccountAsync(long exchangeId, UpdateAccountModel account, CancellationToken cancellationToken);

    /// <summary>
    /// Delete an account.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <returns></returns>
    Task DeleteAccountAsync(long exchangeId, CancellationToken cancellationToken);
}