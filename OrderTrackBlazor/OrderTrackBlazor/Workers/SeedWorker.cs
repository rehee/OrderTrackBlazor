
using Google.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderTrackBlazor.Consts;
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
        db?.Database.Migrate();
        
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
