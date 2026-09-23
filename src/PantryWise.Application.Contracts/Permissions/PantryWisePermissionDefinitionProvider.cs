using PantryWise.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace PantryWise.Permissions;

public class PantryWisePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(PantryWisePermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(PantryWisePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<PantryWiseResource>(name);
    }
}
