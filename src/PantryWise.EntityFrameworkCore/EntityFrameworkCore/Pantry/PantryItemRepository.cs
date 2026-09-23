using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PantryWise.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace PantryWise.Pantry;

public class PantryItemRepository
    : EfCoreRepository<PantryWiseDbContext, PantryItem, Guid>, IPantryItemRepository
{
    public PantryItemRepository(IDbContextProvider<PantryWiseDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<PantryItem>> GetExpiringWithProductAsync(Guid userId, DateTime beforeDate)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.PantryItems
            .Include(i => i.Product)
            .Where(i => i.UserId == userId && i.ExpirationDate <= beforeDate)
            .ToListAsync();
    }
}
