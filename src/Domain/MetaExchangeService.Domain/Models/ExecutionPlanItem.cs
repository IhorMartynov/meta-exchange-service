namespace MetaExchangeService.Domain.Models;

public sealed class ExecutionPlanItem
{
    public Order Order { get; set; }
    public decimal BtcAmount { get; set; }
}