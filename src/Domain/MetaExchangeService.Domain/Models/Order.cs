namespace MetaExchangeService.Domain.Models;

public sealed class Order
{
    public long Id { get; set; }
    public Exchange Exchange { get; set; }
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}