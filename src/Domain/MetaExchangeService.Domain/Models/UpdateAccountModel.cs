namespace MetaExchangeService.Domain.Models;

public sealed class UpdateAccountModel
{
    public decimal? BtcAmount { get; set; }
    public decimal? EurAmount { get; set; }
}