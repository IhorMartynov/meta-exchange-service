using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contracts.Repositories;

namespace MetaExchangeService.Application.Services;

internal sealed class AccountsService : IAccountsService
{
    private readonly IAccountsRepository _accountsRepository;

    public AccountsService(IAccountsRepository accountsRepository) =>
        _accountsRepository = accountsRepository;

    /// <inheritdoc />
    public Task<Account?> GetByExchangeAsync(long exchangeId, CancellationToken cancellationToken) =>
        _accountsRepository.GetByExchangeAsync(exchangeId, cancellationToken);

    /// <inheritdoc />
    public Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken) =>
        _accountsRepository.GetAllAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Account> CreateAsync(CreateAccountModel account, CancellationToken cancellationToken) =>
        _accountsRepository.CreateAsync(account, cancellationToken);

    /// <inheritdoc />
    public Task UpdateAccountAsync(long exchangeId, UpdateAccountModel account, CancellationToken cancellationToken) =>
        _accountsRepository.UpdateAccountAsync(exchangeId, account, cancellationToken);

    /// <inheritdoc />
    public Task DeleteAccountAsync(long exchangeId, CancellationToken cancellationToken) =>
        _accountsRepository.DeleteAccountAsync(exchangeId, cancellationToken);
}