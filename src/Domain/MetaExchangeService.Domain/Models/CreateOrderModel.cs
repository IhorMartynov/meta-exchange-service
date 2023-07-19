namespace MetaExchangeService.Domain.Models;

public sealed class CreateOrderModel
{
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}