using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetaExchangeService.Repositories.Entities;

public sealed class AccountEntity
{
    [Key]
    public long Id { get; set; }
    public long ExchangeId { get; set; }
    [ForeignKey(nameof(ExchangeId))]
    public ExchangeEntity? Exchange { get; set; }
    public decimal BtcAmount { get; set; }
    public decimal EurAmount { get; set; }
}