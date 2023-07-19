namespace MetaExchangeService.Domain.Models;

public sealed class Account
{
    public long Id { get; set; }
    public Exchange Exchange { get; set; }
    public decimal BtcAmount { get; set; }
    public decimal EurAmount { get; set; }

}