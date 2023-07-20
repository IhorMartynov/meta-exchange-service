using MetaExchangeService.Domain.Models;

namespace MetaExchangeService.Application.Services;

public interface IExecutionPlanService
{
    /// <summary>
    /// Get best execution plan to buy BTC.
    /// </summary>
    /// <param name="btcAmount">BTC amount.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<ExecutionPlanItem>> GetBestBuyingPlanAsync(decimal btcAmount, CancellationToken cancellationToken);

    /// <summary>
    /// Get best execution plan to sell BTC.
    /// </summary>
    /// <param name="btcAmount">BTC amount.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<ExecutionPlanItem>> GetBestSellingPlanAsync(decimal btcAmount, CancellationToken cancellationToken);
}