using System.ComponentModel.DataAnnotations;
using MetaExchangeService.Repositories.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace MetaExchangeService.Repositories.Entities;

[Index(nameof(Type))]
[Index(nameof(Price))]
public class OrderEntity
{
    [Key]
    public long Id { get; set; }
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}