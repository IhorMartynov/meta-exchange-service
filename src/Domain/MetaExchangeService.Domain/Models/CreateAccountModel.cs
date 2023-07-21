namespace MetaExchangeService.Domain.Models;

public sealed class CreateAccountModel
{
    public string? ExchangeName { get; set; }
    public decimal BtcAmount { get; set; }
    public decimal EurAmount { get; set; }
}