using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PantryWise.Pantry;

public interface IPantryQueryAppService : IApplicationService
{
    Task<List<PantryItemDto>> GetExpiringItemsAsync(DateTime beforeDate);
}
