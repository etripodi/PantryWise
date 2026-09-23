using System;
using Volo.Abp.Domain.Entities;

namespace PantryWise.Pantry;

public class PantryItem : AggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public DateTime ExpirationDate { get; set; }
}
