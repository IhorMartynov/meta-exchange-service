using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contracts.Repositories;

namespace MetaExchangeService.Application.Services;

internal sealed class ExecutionPlanService : IExecutionPlanService
{
    private const int PageSize = 10;

    private readonly IAccountsRepository _accountsRepository;
    private readonly IOrdersRepository _ordersRepository;

    public ExecutionPlanService(IAccountsRepository accountsRepository,
        IOrdersRepository ordersRepository)
    {
        _accountsRepository = accountsRepository;
        _ordersRepository = ordersRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExecutionPlanItem>> GetBestBuyingPlanAsync(decimal btcAmount, CancellationToken cancellationToken)
    {
        if (btcAmount <= 0) return Enumerable.Empty<ExecutionPlanItem>();

        var accounts = (await _accountsRepository.GetAllAsync(cancellationToken))
            .ToDictionary(x => x.Exchange.Id, x => x.EurAmount);

        var btcAmountLeft = btcAmount;
        var result = new List<ExecutionPlanItem>();
        var page = 1;

        while (btcAmountLeft > 0 && accounts.Sum(x => x.Value) > 0)
        {
            var ordersPage = (await _ordersRepository.GetAskOrdersSortedByPriceAscendingAsync(page, PageSize, cancellationToken))
                .ToArray();

            if (ordersPage.Length <= 0) return result;

            foreach (var order in ordersPage)
            {
                if (accounts[order.Exchange.Id] <= 0) continue;
                if (btcAmountLeft <= 0) return result;

                var btcCanBuy = accounts[order.Exchange.Id] / order.Price;

                var btcToBuy = new[] {order.Amount, btcAmountLeft, btcCanBuy}.Min();

                result.Add(new ExecutionPlanItem {Order = order, BtcAmount = btcToBuy});
                btcAmountLeft -= btcToBuy;
                accounts[order.Exchange.Id] -= btcToBuy * order.Price;
            }

            page++;
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ExecutionPlanItem>> GetBestSellingPlanAsync(decimal btcAmount, CancellationToken cancellationToken)
    {
        if (btcAmount <= 0) return Enumerable.Empty<ExecutionPlanItem>();

        var accounts = (await _accountsRepository.GetAllAsync(cancellationToken))
            .ToDictionary(x => x.Exchange.Id, x => x.BtcAmount);

        var btcAmountLeft = btcAmount;
        var result = new List<ExecutionPlanItem>();
        var page = 1;

        while (btcAmountLeft > 0 && accounts.Sum(x => x.Value) > 0)
        {
            var ordersPage = (await _ordersRepository.GetBidOrdersSortedByPriceDescendingAsync(page, PageSize, cancellationToken))
                .ToArray();

            if (ordersPage.Length <= 0) return result;

            foreach (var order in ordersPage)
            {
                if (accounts[order.Exchange.Id] <= 0) continue;
                if (btcAmountLeft <= 0) return result;

                var btcCanSell = accounts[order.Exchange.Id];

                var btcToSell = new[] { order.Amount, btcAmountLeft, btcCanSell }.Min();

                result.Add(new ExecutionPlanItem { Order = order, BtcAmount = btcToSell });
                btcAmountLeft -= btcToSell;
                accounts[order.Exchange.Id] -= btcToSell;
            }

            page++;
        }

        return result;
    }
}