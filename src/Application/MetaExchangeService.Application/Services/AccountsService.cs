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
}