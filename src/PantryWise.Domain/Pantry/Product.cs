using System;
using Volo.Abp.Domain.Entities;

namespace PantryWise.Pantry;

// Versión mínima para que compilen los ejemplos de repositorio; RF-05..RF-09 agregan el resto.
public class Product : AggregateRoot<Guid>
{
    public string BarCode { get; set; } = default!;
    public string Name { get; set; } = default!;
}
