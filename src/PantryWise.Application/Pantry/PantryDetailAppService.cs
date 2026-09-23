using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Users;

namespace PantryWise.Pantry;

[Authorize]
public class PantryDetailAppService : PantryWiseAppService, IPantryDetailAppService
{
    private readonly IPantryItemRepository _itemRepository;

    public PantryDetailAppService(IPantryItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<List<PantryItemDto>> GetExpiringWithProductAsync(DateTime beforeDate)
    {
        var currentUserId = CurrentUser.GetId();
        var items = await _itemRepository.GetExpiringWithProductAsync(currentUserId, beforeDate);
        return ObjectMapper.Map<List<PantryItem>, List<PantryItemDto>>(items);
    }
}
