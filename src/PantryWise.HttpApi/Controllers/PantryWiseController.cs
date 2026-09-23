using PantryWise.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PantryWise.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class PantryWiseController : AbpControllerBase
{
    protected PantryWiseController()
    {
        LocalizationResource = typeof(PantryWiseResource);
    }
}
