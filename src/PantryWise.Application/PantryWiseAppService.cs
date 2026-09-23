using PantryWise.Localization;
using Volo.Abp.Application.Services;

namespace PantryWise;

/* Inherit your application services from this class.
 */
public abstract class PantryWiseAppService : ApplicationService
{
    protected PantryWiseAppService()
    {
        LocalizationResource = typeof(PantryWiseResource);
    }
}
