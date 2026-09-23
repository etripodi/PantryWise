using PantryWise.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PantryWise.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PantryWiseEntityFrameworkCoreModule),
    typeof(PantryWiseApplicationContractsModule)
)]
public class PantryWiseDbMigratorModule : AbpModule
{
}
