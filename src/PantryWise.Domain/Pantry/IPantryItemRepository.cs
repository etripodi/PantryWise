using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace PantryWise.Pantry;

public interface IPantryItemRepository : IRepository<PantryItem, Guid>
{
    Task<List<PantryItem>> GetExpiringWithProductAsync(Guid userId, DateTime beforeDate);
}
