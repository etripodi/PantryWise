using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace PantryWise.Pantry;

/* Datos de ejemplo para probar las cuatro variantes de repositorio.
 * Corre desde PantryWise.DbMigrator. Las fechas son relativas a la ejecución del seed.
 */
public class PantryDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    // Usuario ficticio (no existe en AbpUsers) para comprobar que el filtro por UserId excluye sus ítems.
    private static readonly Guid OtherUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<PantryItem, Guid> _pantryItemRepository;
    private readonly IIdentityUserRepository _userRepository;

    public PantryDataSeedContributor(
        IRepository<Product, Guid> productRepository,
        IRepository<PantryItem, Guid> pantryItemRepository,
        IIdentityUserRepository userRepository)
    {
        _productRepository = productRepository;
        _pantryItemRepository = pantryItemRepository;
        _userRepository = userRepository;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        if (context.TenantId != null || await _productRepository.GetCountAsync() > 0)
        {
            return;
        }

        var admin = await _userRepository.FindByNormalizedUserNameAsync("ADMIN");
        if (admin == null)
        {
            return;
        }

        var milk = new Product { BarCode = "7790580100014", Name = "Leche entera" };
        var yogurt = new Product { BarCode = "7791337002419", Name = "Yogur bebible frutilla" };
        var cheese = new Product { BarCode = "7790787000261", Name = "Queso cremoso" };
        var ham = new Product { BarCode = "7790360026596", Name = "Jamón cocido feteado" };
        var bread = new Product { BarCode = "7792200000128", Name = "Pan lactal" };
        var rice = new Product { BarCode = "7790070318107", Name = "Arroz largo fino" };
        var pasta = new Product { BarCode = "7790070012043", Name = "Fideos spaghetti" };
        var tomatoPuree = new Product { BarCode = "7790580508209", Name = "Puré de tomate" };

        // InsertManyAsync asigna los Id (Guid) antes de guardar.
        await _productRepository.InsertManyAsync(
            new[] { milk, yogurt, cheese, ham, bread, rice, pasta, tomatoPuree }, autoSave: true);

        var today = DateTime.Today;
        await _pantryItemRepository.InsertManyAsync(new List<PantryItem>
        {
            Item(admin.Id, ham, today.AddDays(-2)),   // ya vencido
            Item(admin.Id, milk, today.AddDays(1)),
            Item(admin.Id, yogurt, today.AddDays(3)),
            Item(admin.Id, bread, today.AddDays(5)),
            Item(admin.Id, cheese, today.AddDays(10)),
            Item(admin.Id, tomatoPuree, today.AddDays(90)),
            Item(admin.Id, pasta, today.AddDays(365)),
            Item(admin.Id, rice, today.AddDays(540)),
            Item(OtherUserId, milk, today.AddDays(2)),  // de otro usuario: nunca debe aparecer para admin
            Item(OtherUserId, cheese, today.AddDays(4)),
        }, autoSave: true);
    }

    private static PantryItem Item(Guid userId, Product product, DateTime expirationDate) =>
        new() { UserId = userId, ProductId = product.Id, ExpirationDate = expirationDate };
}
