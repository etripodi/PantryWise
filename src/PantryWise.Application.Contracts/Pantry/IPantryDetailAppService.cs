using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PantryWise.Pantry;

public interface IPantryDetailAppService : IApplicationService
{
    Task<List<PantryItemDto>> GetExpiringWithProductAsync(DateTime beforeDate);
}
