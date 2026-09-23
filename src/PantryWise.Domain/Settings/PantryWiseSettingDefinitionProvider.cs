using Volo.Abp.Settings;

namespace PantryWise.Settings;

public class PantryWiseSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(PantryWiseSettings.MySetting1));
    }
}
