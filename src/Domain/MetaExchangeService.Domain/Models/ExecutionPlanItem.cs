namespace MetaExchangeService.Domain.Models;

public sealed class ExecutionPlanItem
{
    public Order Order { get; set; } = null!;
    public decimal BtcAmount { get; set; }
}