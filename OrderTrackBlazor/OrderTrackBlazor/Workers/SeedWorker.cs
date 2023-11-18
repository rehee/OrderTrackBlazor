
using Google.Api;
using Microsoft.Extensions.Logging;
using OrderTrackBlazor.Data;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;
using ReheeCmf.Enums;
using ReheeCmf.MultiTenants;

namespace OrderTrackBlazor.Workers
{
  public class SeedWorker : BackgroundService
  {
    private readonly IServiceProvider sp;

    public SeedWorker(IServiceProvider sp)
    {
      this.sp = sp;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      using var scope = sp.CreateScope();
      var option = scope.ServiceProvider.GetService<CrudOption>();
      if (option?.SQLType == EnumSQLType.Memory)
      {
        return;
      }
      try
      {
        using var context = scope.ServiceProvider.GetService<IContext>();
        var db = context.Context as ApplicationDbContext;

        Array enumValues = Enum.GetValues(typeof(EnumShop));
        foreach (var e in enumValues)
        {
          if (e is EnumShop ev)
          {
            if (!db.OrderTrackShops.Any(b => b.ShopType == ev && String.IsNullOrEmpty(b.PostCode)))
            {
              await db.AddAsync<OrderTrackShop>(new OrderTrackShop()
              {
                ShopType = ev
              }, stoppingToken);
            }
          }
        }
        await context.SaveChangesAsync(null, stoppingToken);
      }
      catch (Exception ex)
      {

        throw ex;
      }
    }
  }
}
