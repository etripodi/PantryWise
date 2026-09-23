using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PantryWise.Data;

/* This is used if database provider does't define
 * IPantryWiseDbSchemaMigrator implementation.
 */
public class NullPantryWiseDbSchemaMigrator : IPantryWiseDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
