using System.Threading.Tasks;

namespace PantryWise.Data;

public interface IPantryWiseDbSchemaMigrator
{
    Task MigrateAsync();
}
