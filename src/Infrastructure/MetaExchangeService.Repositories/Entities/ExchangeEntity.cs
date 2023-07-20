﻿using System.ComponentModel.DataAnnotations;

namespace MetaExchangeService.Repositories.Entities;

public sealed class ExchangeEntity
{
    [Key]
    public long Id { get; set; }

    public string Name { get; set; }
}