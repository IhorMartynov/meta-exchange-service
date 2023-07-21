using System.ComponentModel.DataAnnotations;
using Mapster;
using MetaExchangeService.Domain.Exceptions;
using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contexts;
using MetaExchangeService.Repositories.Contracts.Repositories;
using MetaExchangeService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetaExchangeService.Repositories.Repositories;

internal sealed class AccountsRepository : IAccountsRepository
{
    private readonly OrderBookContext _context;

    public AccountsRepository(OrderBookContext context) =>
        _context = context;

    /// <inheritdoc />
    public async Task<Account?> GetByExchangeAsync(long exchangeId, CancellationToken cancellationToken)
    {
        _ = await _context.Exchanges.FirstOrDefaultAsync(x => x.Id == exchangeId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(ExchangeEntity), exchangeId);

        var entity = await _context.Accounts
            .Include(x => x.Exchange)
            .FirstOrDefaultAsync(x => x.ExchangeId == exchangeId, cancellationToken);

        return entity?.Adapt<Account>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .Include(x => x.Exchange)
            .ToArrayAsync(cancellationToken);

        return entity.Select(x => x.Adapt<Account>());
    }

    /// <inheritdoc />
    public async Task<Account> CreateAsync(CreateAccountModel account, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(account.ExchangeName))
            throw new ValidationException("The name of an exchange cannot be empty.");

        var exchangeEntity = new ExchangeEntity {Name = account.ExchangeName};
        await _context.Exchanges.AddAsync(exchangeEntity, cancellationToken);

        var accountEntity = new AccountEntity
        {
            Exchange = exchangeEntity,
            BtcAmount = account.BtcAmount,
            EurAmount = account.EurAmount
        };
        await _context.Accounts.AddAsync(accountEntity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return accountEntity.Adapt<Account>();
    }

    /// <inheritdoc />
    public async Task UpdateAccountAsync(long exchangeId, UpdateAccountModel account, CancellationToken cancellationToken)
    {
        _ = await _context.Exchanges.FirstOrDefaultAsync(x => x.Id == exchangeId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(ExchangeEntity), exchangeId);

        var entity = await _context.Accounts
                         .FirstOrDefaultAsync(x => x.ExchangeId == exchangeId, cancellationToken)
                     ?? throw new EntityNotFoundException(nameof(AccountEntity), exchangeId);

        entity.BtcAmount = account.BtcAmount ?? entity.BtcAmount;
        entity.EurAmount = account.EurAmount ?? entity.EurAmount;

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAccountAsync(long exchangeId, CancellationToken cancellationToken)
    {
        var exchangeEntity = await _context.Exchanges.FirstOrDefaultAsync(x => x.Id == exchangeId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(ExchangeEntity), exchangeId);

        var accountEntity = await _context.Accounts
                         .FirstOrDefaultAsync(x => x.ExchangeId == exchangeId, cancellationToken)
                     ?? throw new EntityNotFoundException(nameof(AccountEntity), exchangeId);

        await _context.Orders
            .Where(x => x.ExchangeId == exchangeId)
            .ExecuteDeleteAsync(cancellationToken);

        _context.Accounts.Remove(accountEntity);

        _context.Exchanges.Remove(exchangeEntity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}