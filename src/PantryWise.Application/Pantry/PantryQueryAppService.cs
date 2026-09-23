using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace PantryWise.Pantry;

[Authorize]
public class PantryQueryAppService : PantryWiseAppService, IPantryQueryAppService
{
    private readonly IReadOnlyRepository<PantryItem, Guid> _itemRepository;

    public PantryQueryAppService(IReadOnlyRepository<PantryItem, Guid> itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<List<PantryItemDto>> GetExpiringItemsAsync(DateTime beforeDate)
    {
        var currentUserId = CurrentUser.GetId();
        var items = await _itemRepository.GetListAsync(
            i => i.UserId == currentUserId && i.ExpirationDate <= beforeDate);
        return ObjectMapper.Map<List<PantryItem>, List<PantryItemDto>>(items);
    }
}
