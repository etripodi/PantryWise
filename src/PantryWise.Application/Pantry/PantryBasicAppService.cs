using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace PantryWise.Pantry;

[Authorize]
public class PantryBasicAppService : PantryWiseAppService, IPantryBasicAppService
{
    private readonly IBasicRepository<PantryItem, Guid> _itemRepository;

    public PantryBasicAppService(IBasicRepository<PantryItem, Guid> itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<List<PantryItemDto>> GetExpiringItemsAsync(DateTime beforeDate)
    {
        var currentUserId = CurrentUser.GetId();

        // IBasicRepository no expone IQueryable: no hay Where() contra la base.
        var allItems = await _itemRepository.GetListAsync();
        var items = allItems
            .Where(i => i.UserId == currentUserId && i.ExpirationDate <= beforeDate)
            .ToList();
        return ObjectMapper.Map<List<PantryItem>, List<PantryItemDto>>(items);
    }
}
