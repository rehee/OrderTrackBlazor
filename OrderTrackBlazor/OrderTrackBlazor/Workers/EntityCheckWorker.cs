using Microsoft.EntityFrameworkCore;
using ReheeCmf.Commons;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Workers
{
  public class EntityCheckWorker : BackgroundService
  {
    private readonly IServiceProvider sp;

    public EntityCheckWorker(IServiceProvider sp)
    {
      this.sp = sp;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        while (true)
        {
          
          await Task.Delay(1000 * 60);
          using var scope = sp.CreateScope();
          using var context = scope.ServiceProvider.GetService<IContext>();
          var current = DateTime.UtcNow;
          var nextCurrent = DateTime.UtcNow.AddMinutes(15);

          var dispatchs = await context.Query<OrderTrackDispatchRecord>(false)
            .Where(b =>
              (b.SoftDeleteUntil != null && b.SoftDeleteUntil < current) ||
              (b.Status == EnumDispatchStatus.Error &&
                (
                  (b.UpdateDate != null && b.UpdateDate > nextCurrent) ||
                  b.CreateDate > nextCurrent)
                )

            ).ToListAsync();
          foreach (var d in dispatchs)
          {
            context.Delete<OrderTrackDispatchRecord>(d);
          }
          await context.SaveChangesAsync(null);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
