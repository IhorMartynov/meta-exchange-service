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
}