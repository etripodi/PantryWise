using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PantryWise.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class PantryWiseDbContextFactory : IDesignTimeDbContextFactory<PantryWiseDbContext>
{
    public PantryWiseDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        PantryWiseEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<PantryWiseDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new PantryWiseDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PantryWise.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
