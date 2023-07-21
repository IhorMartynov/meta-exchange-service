using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MetaExchangeService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaExchangeService.Repositories.Entities;

[Index(nameof(Type))]
[Index(nameof(Price))]
public sealed class OrderEntity
{
    [Key]
    public long Id { get; set; }
    public long ExchangeId { get; set; }
    [ForeignKey(nameof(ExchangeId))]
    public ExchangeEntity? Exchange { get; set; }
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}