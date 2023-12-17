using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.ContextModule.Contexts;

namespace OrderTrackBlazor.Data
{
  public class ApplicationDbContext : CmfIdentityContext<ApplicationUser>
  {
    public ApplicationDbContext(IServiceProvider sp) : base(sp)
    {
    }

    public DbSet<OrderTrackDispatchItem> OrderTrackDispatchItems { get; set; }
    public DbSet<OrderTrackDispatchRecord> OrderTrackDispatchRecords { get; set; }
    public DbSet<OrderTrackOrder> OrderTrackOrders { get; set; }
    public DbSet<OrderTrackOrderItem> OrderTrackOrderItems { get; set; }
    public DbSet<OrderTrackProduction> OrderTrackProductions { get; set; }
    public DbSet<OrderTrackPurchaseItem> OrderTrackPurchaseItems { get; set; }
    public DbSet<OrderTrackPurchaseRecord> OrderTrackPurchaseRecords { get; set; }
    public DbSet<OrderTrackShop> OrderTrackShops { get; set; }
    public DbSet<OrderTrackDispatchPackage> OrderTrackDispatchPackages { get; set; }
    public DbSet<OrderTrackPackage> OrderTrackPackages { get; set; }
    public DbSet<OrderTrackPackageItem> OrderTrackPackageItems { get; set; }
    public DbSet<OrderTrackPackageSize> OrderTrackPackageSizes { get; set; }
  }
}
