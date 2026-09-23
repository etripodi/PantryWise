using Microsoft.Extensions.Localization;
using PantryWise.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PantryWise;

[Dependency(ReplaceServices = true)]
public class PantryWiseBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<PantryWiseResource> _localizer;

    public PantryWiseBrandingProvider(IStringLocalizer<PantryWiseResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
