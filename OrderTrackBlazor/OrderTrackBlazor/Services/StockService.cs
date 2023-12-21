
using Dropbox.Api.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
            production.PurchaseItems.Sum(b => b.Quantity) - production.DispatchItems.Where(b => b.DispatchRecord.Status != EnumDispatchStatus.Error).Sum(b => b.Quantity + b.PackageQuantity),
        };
    }
    public IQueryable<StockListDTO> QueryDetail()
    {
      var purchaesQuery =
        from purchaes in context.Query<OrderTrackPurchaseItem>(true).Where(b => b.Quantity != 0)
        select new StockListDTO
        {
          Pk = purchaes.Id,
          Id = purchaes.ProductionId,
          //Name = purchaes.Production.Name,
          CreateDate = purchaes.CreateDate,
          Date = purchaes.PurchaseRecord.PurchaseDate,
          IsPurchase = true,
          Number = purchaes.Quantity,
          Shop = purchaes.PurchaseRecord.Shop.Name,
          OrderShortNote = ""
        };
      var dispatchQuery =
        from dispatch in context.Query<OrderTrackDispatchItem>(true).Where(b => b.Quantity != 0 || b.PackageQuantity != 0)
        join record in context.Query<OrderTrackDispatchRecord>(true)
          .Where(b => b.Status != EnumDispatchStatus.Error) on dispatch.DispatchRecordId equals record.Id
        select new StockListDTO
        {
          Pk = dispatch.Id,
          Id = dispatch.ProductionId,
          CreateDate = dispatch.CreateDate,
          Date = record.DispatchDate,
          IsPurchase = false,
          Number = dispatch.Quantity + dispatch.PackageQuantity,
          OrderShortNote = record.Order.ShortNote,
          //Name = dispatch.Production.Name,
          Shop = "Other"
        };
      var stockDispatch =
        from dispatch in context.Query<OrderTrackDispatchItem>(true).Where(b => b.PackageQuantity != 0)
        join record in context.Query<OrderTrackStockDispatchPackage>(true) on dispatch.OrderTrackStockDispatchPackageId equals record.Id
        select new StockListDTO
        {
          Pk = dispatch.Id,
          Id = dispatch.ProductionId,
          CreateDate = dispatch.CreateDate,
          Date = record.Dispatch.DispatchDate,
          IsPurchase = false,
          Number = dispatch.PackageQuantity,
          OrderShortNote = record.BriefDiscribtion,
          //Name = dispatch.Production.Name,
          Shop = "Other"
        };
      return purchaesQuery.Concat(dispatchQuery).Concat(stockDispatch);

    }

    public IQueryable<StockRequireDTO> QueryAvaliable()
    {
      var query =
        from orderItem in context.Query<OrderTrackOrderItem>(true)
        .Where(b =>
          b.Quantity >
            b.DispatchItems.Where(b =>
              b.DispatchRecord == null || b.DispatchRecord.Status != EnumDispatchStatus.Error)
            .Sum(b => b.Quantity + b.PackageQuantity)
            )
        select new StockRequireDTO
        {
          OrderItemId = orderItem.Id,
          OrderDate = orderItem.Order.OrderDate,
          Note = orderItem.Note,
          OrderPrice = orderItem.OrderPrice == null ? orderItem.Production.OriginalPrice : orderItem.OrderPrice,
          ProductionId = orderItem.ProductionId,
          ProductionName = orderItem.Production.Name,
          RecommandShopId = orderItem.RecommendShopId,
          RecommandShopName = orderItem.RecommendShop.Name,
          RequiredNumber = orderItem.Quantity,
          DispatchNumber = orderItem.DispatchItems.Where(b =>
              b.DispatchRecord == null || b.DispatchRecord.Status != EnumDispatchStatus.Error)
            .Sum(b => b.Quantity + b.PackageQuantity),
          StockNumber = orderItem.Production.PurchaseItems.Sum(b => b.Quantity) -
            orderItem.Production.DispatchItems.Where(b =>
              b.DispatchRecord == null || b.DispatchRecord.Status != EnumDispatchStatus.Error)
            .Sum(b => b.Quantity + b.PackageQuantity)
        };
      return query;
    }

    public IQueryable<StockRequireSummaryDTO> QueryRequireSummary(long? purchaseId = null)
    {
      var query =
        from production in context.Query<OrderTrackProduction>(true)
        let required = production.OrderItems.Sum(b => b.Quantity)
        let dispatch = production.DispatchItems.Where(b =>
              (b.DispatchRecord != null && b.DispatchRecord.Status != EnumDispatchStatus.Error) ||
              (b.OrderTrackStockDispatchPackage != null &&
               b.OrderTrackStockDispatchPackage.Dispatch != null &&
               b.OrderTrackStockDispatchPackage.Dispatch.Status != EnumDispatchStatus.Error))
            .Sum(b => b.Quantity + b.PackageQuantity)
        let purchase = production.PurchaseItems.Where(b => purchaseId.HasValue ? b.PurchaseRecordId != purchaseId : true).Sum(b => b.Quantity)
        let stock = purchase - dispatch
        let items = production.OrderItems.Select(b =>
          new StockRequireDTO
          {
            OrderItemId = b.Id,
            OrderId = b.OrderTrackOrderId,
            OrderNote = b.Order.ShortNote,
            Note = b.Note,
            OrderDate = b.Order.OrderDate,
            OrderPrice = b.OrderPrice == null ? b.OrderPrice : production.OriginalPrice,
            ProductionId = production.Id,
            ProductionName = production.Name,
            RecommandShopId = b.RecommendShopId,
            RecommandShopName = b.RecommendShop.Name,
            RequiredNumber = b.Quantity,
            StockNumber = stock,
            DispatchNumber = b.DispatchItems.Where(b =>
              (b.DispatchRecord != null && b.DispatchRecord.Status != EnumDispatchStatus.Error) ||
              (b.OrderTrackStockDispatchPackage != null &&
               b.OrderTrackStockDispatchPackage.Dispatch != null &&
               b.OrderTrackStockDispatchPackage.Dispatch.Status != EnumDispatchStatus.Error))
            .Sum(b => b.Quantity + b.PackageQuantity)
          }).Where(b => b.RequiredNumber > b.DispatchNumber)


        select new StockRequireSummaryDTO
        {
          ProductionId = production.Id,
          ProductionName = production.Name,
          RequiredNumber = required,
          DispatchNumber = dispatch,
          StockNumber = stock,
          PurchaseNumber = purchase,
          PendingNumber = required - dispatch,
          Items = items
        };

      return query.Where(b => b.PendingNumber > b.StockNumber && b.PendingNumber > 0);
    }

    public async Task<StockPurchaseDTO> FindStockPurchase(long purchaseId)
    {
      var result = await (
        from record in context.Query<OrderTrackPurchaseRecord>(true).Where(b => b.Id == purchaseId)
        select new StockPurchaseDTO
        {
          PurchaseId = record.Id,
          Price = record.Price,
          PurchaseDate = record.PurchaseDate,
          ShopId = record.ShopId,
          Request = record.Items.Select(b => new StockRequireSummaryDTO
          {
            ProductionId = b.ProductionId,
            ProductionName = b.Production.Name,
            Number = b.Quantity,
          })
        }).FirstOrDefaultAsync();
      if (result == null)
      {
        return new StockPurchaseDTO { };
      }
      result.Request = result.Request
        .GroupBy(b => b.ProductionId)
        .Select(b => new StockRequireSummaryDTO
        {
          ProductionId = b.Key,
          ProductionName = b.Select(b => b.ProductionName).FirstOrDefault(),
          Number = b.Sum(b => b.Number)
        }).Select(b => new StockRequireSummaryDTO
        {
          ProductionId = b.ProductionId,
          ProductionName = b.ProductionName,
          Number = b.Number == 0 ? null : b.Number,
        }).ToArray();
      return result;
    }

    public async Task<bool> CreateStockPurchase(StockPurchaseDTO stockPurchase)
    {
      var purchase = new OrderTrackPurchaseRecord
      {
        PurchaseDate = stockPurchase.PurchaseDate?.Date ?? DateTime.UtcNow.Date,
        Price = stockPurchase.Price,
        ShopId = stockPurchase.ShopId,
      };
      await context.AddAsync(purchase, CancellationToken.None);
      foreach (var item in stockPurchase.Request ?? Enumerable.Empty<StockRequireSummaryDTO>())
      {
        var purchaseItem = new OrderTrackPurchaseItem
        {
          ProductionId = item.ProductionId,
          PurchaseRecordId = purchase.Id,
          PurchaseRecord = purchase,
          Quantity = item.Number ?? 0,
        };
        await context.AddAsync(purchaseItem, CancellationToken.None);
      };
      await context.SaveChangesAsync(null);
      return true;
    }

    public async Task<bool> UpdateStockPurchase(StockPurchaseDTO stockPurchase)
    {
      var record = await context.Query<OrderTrackPurchaseRecord>(false).Where(b => b.Id == stockPurchase.PurchaseId).FirstOrDefaultAsync();
      if (record == null)
      {
        return true;
      }
      record.ShopId = stockPurchase.ShopId;
      record.PurchaseDate = stockPurchase.PurchaseDate;
      record.Price = stockPurchase.Price;
      var items = record.Items.ToArray().GroupBy(b => b.ProductionId).ToArray();
      foreach (var dto in stockPurchase.Request)
      {
        var itemfound = items.FirstOrDefault(b => b.Key == dto.ProductionId);
        if (itemfound == null)
        {
          if ((dto.ProductionId ?? 0) <= 0)
          {
            continue;
          }
          await context.AddAsync(new OrderTrackPurchaseItem
          {
            PurchaseRecordId = record.Id,
            Quantity = dto.Number ?? 0,
            ProductionId = dto.ProductionId
          }, CancellationToken.None);
        }
        else
        {
          var itemList = itemfound.ToArray();
          for (var i = 0; i < itemList.Length; i++)
          {
            var entityItem = itemList[i];
            if (i == 0)
            {
              entityItem.Quantity = dto.Number ?? 0;
              continue;
            }
            context.Delete(entityItem);
          }
        }
      }
      await context.SaveChangesAsync(null);
      return true;
    }
  }
}
