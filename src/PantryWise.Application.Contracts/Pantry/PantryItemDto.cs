using System;
using Volo.Abp.Application.Dtos;

namespace PantryWise.Pantry;

public class PantryItemDto : EntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class ProductDto : EntityDto<Guid>
{
    public string BarCode { get; set; } = default!;
    public string Name { get; set; } = default!;
}
