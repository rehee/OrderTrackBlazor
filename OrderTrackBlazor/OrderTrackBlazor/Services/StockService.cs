
using Dropbox.Api.Users;
using Microsoft.EntityFrameworkCore;
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
          CurrentStock =
            production.PurchaseItems.Sum(b => b.Quantity) - production.DispatchItems.Where(b => b.DispatchRecord.Status != EnumDispatchStatus.Error).Sum(b => b.Quantity),
        };
    }
    public IQueryable<StockListDTO> QueryDetail()
    {
      var purchaesQuery =
        from purchaes in context.Query<OrderTrackPurchaseItem>(true)
        select new StockListDTO
        {
          Pk = purchaes.Id,
          Id = purchaes.ProductionId,
          //Name = purchaes.Production.Name,
          CreateDate = purchaes.CreateDate,
          Date = purchaes.PurchaseRecord.PurchaseDate,
          IsPurchase = true,
          Number = purchaes.Quantity,
          Shop = purchaes.PurchaseRecord.Shop.ShopType,
          OrderShortNote = ""
        };
      var dispatchQuery =
        from dispatch in context.Query<OrderTrackDispatchItem>(true)
        join record in context.Query<OrderTrackDispatchRecord>(true)
          .Where(b => b.Status != EnumDispatchStatus.Error) on dispatch.DispatchRecordId equals record.Id
        select new StockListDTO
        {
          Pk = dispatch.Id,
          Id = dispatch.ProductionId,
          CreateDate = dispatch.CreateDate,
          Date = record.DispatchDate,
          IsPurchase = false,
          Number = dispatch.Quantity,
          OrderShortNote = record.Order.ShortNote,
          //Name = dispatch.Production.Name,
          Shop = EnumShop.Others
        };
      return purchaesQuery.Concat(dispatchQuery);

    }
  }
}
