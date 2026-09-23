using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PantryWise.Data;
using Volo.Abp.DependencyInjection;

namespace PantryWise.EntityFrameworkCore;

public class EntityFrameworkCorePantryWiseDbSchemaMigrator
    : IPantryWiseDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorePantryWiseDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the PantryWiseDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<PantryWiseDbContext>()
            .Database
            .MigrateAsync();
    }
}
