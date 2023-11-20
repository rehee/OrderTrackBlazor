
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class StockService : IStockService
  {
    private readonly IContext context;

    public StockService(IContext context)
    {
      this.context = context;
    }
    public IQueryable<StockSummaryDTO> QuerySummary()
    {
      return
        from production in context.Query<OrderTrackProduction>(true)
        select new StockSummaryDTO
        {
          Id = production.Id,
          Name = production.Name,
          CurrentStock = production.PurchaseItems.Sum(b => b.Quantity) - production.DispatchItems.Sum(b => b.Quantity),
        };
    }
  }
}
