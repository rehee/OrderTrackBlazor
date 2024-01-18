
using Dropbox.Api.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrderTrackBlazor.Components.Pages;
using OrderTrackBlazor.Consts;
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
          Image = production.AttachmentId,
          CategoryName = production.Category.Name,
          CategoryDisplayOrder = production.Category.DisplayOrder,
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
          IsPurchase = purchaes.PurchaseRecord.Shop != null ? purchaes.PurchaseRecord.Shop.Name == DefaultValues.VirtualShop ? purchaes.Quantity >= 0 ? EnumInOutStatus.VirtualInput : EnumInOutStatus.VirtualOutPut : EnumInOutStatus.Input : EnumInOutStatus.Input,
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
          IsPurchase = EnumInOutStatus.OutPut,
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
          IsPurchase = EnumInOutStatus.OutPut,
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
          RecommandShopId2 = orderItem.RecommendShopId2,
          RecommandShopName2 = orderItem.RecommendShop2.Name,
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

    public IQueryable<StockRequireSummaryDTO> QueryRequireSummary(long? purchaseId = null, bool showAvaliableOnly = true)
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
        let orderDispatch = production.OrderItems.Sum(b => b.OverDeliveredNumber)

        let items = production.OrderItems.Select(o =>
          new StockRequireDTO
          {
            OrderItemId = o.Id,
            OrderId = o.OrderTrackOrderId,
            OrderNote = o.Order.ShortNote,
            Note = o.Note,
            OrderDate = o.Order.OrderDate,
            OrderPrice = o.OrderPrice != null ? o.OrderPrice : production.OriginalPrice,
            ProductionId = production.Id,
            ProductionName = production.Name,
            RecommandShopId = o.RecommendShopId,
            RecommandShopName = o.RecommendShop.Name,
            RecommandShopId2 = o.RecommendShopId2,
            RecommandShopName2 = o.RecommendShop2.Name,
            RequiredNumber = o.Quantity,
            StockNumber = stock,
            DispatchNumber = o.DispatchItems.Where(d =>
              (d.DispatchRecord != null && d.DispatchRecord.Status != EnumDispatchStatus.Error) ||
              (d.OrderTrackStockDispatchPackage != null &&
               d.OrderTrackStockDispatchPackage.Dispatch != null &&
               d.OrderTrackStockDispatchPackage.Dispatch.Status != EnumDispatchStatus.Error))
            .Sum(s => s.Quantity + s.PackageQuantity)
          })


        select new StockRequireSummaryDTO
        {
          ProductionId = production.Id,
          ProductionName = production.Name,
          ExtendUrl = production.ExtendUrl,
          RequiredNumber = required,
          DispatchNumber = dispatch,
          StockNumber = stock,
          PurchaseNumber = purchase,
          PendingNumber = required - orderDispatch,
          Items = items,
          CategoryName = production.Category.Name,
          CategoryDisplayOrder = production.Category.DisplayOrder,

        };
      if (showAvaliableOnly != true)
      {
        return query;
      }
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
            ExtendUrl = b.Production.ExtendUrl,
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
          ExtendUrl = b.Select(b => b.ExtendUrl).FirstOrDefault(),
          Number = b.Sum(b => b.Number)
        }).Select(b => new StockRequireSummaryDTO
        {
          ProductionId = b.ProductionId,
          ProductionName = b.ProductionName,
          ExtendUrl = b.ExtendUrl,
          Number = b.Number == 0 ? null : b.Number,
        }).OrderByDescending(b => b.Number.HasValue && b.Number > 0).ThenBy(b => b.ProductionName).ToArray();
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
      foreach (var item in stockPurchase.RequestAdding)
      {
        var purchaseItem = new OrderTrackPurchaseItem
        {
          ProductionId = item.ProductionId,
          PurchaseRecordId = purchase.Id,
          PurchaseRecord = purchase,
          Quantity = item.Number ?? 0,
        };
        if (purchaseItem.Quantity == 0)
        {
          continue;
        }
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
      foreach (var dto in stockPurchase.RequestAdding)
      {
        var itemfound = items.FirstOrDefault(b => b.Key == dto.ProductionId);
        if (itemfound == null)
        {
          if ((dto.ProductionId ?? 0) <= 0)
          {
            continue;
          }
          if ((dto.Number ?? 0) == 0)
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

    public async Task<bool> UpdateStockOrderItem(IEnumerable<StockRequireDTO> dtos)
    {
      var ids = dtos.Select(b => b.OrderItemId).ToArray();
      if (ids?.Any() != true)
      {
        return false;
      }
      var orderItems = await context.Query<OrderTrackOrderItem>(false).Where(b => ids.Contains(b.Id) == true).ToArrayAsync();
      foreach (var item in orderItems)
      {
        var selectedDTO = dtos.FirstOrDefault(b => b.OrderItemId == item.Id);
        if (selectedDTO == null)
        {
          continue;
        }
        item.Quantity = selectedDTO.RequiredNumber;
        item.RecommendShopId = selectedDTO.RecommandShopId;
        item.RecommendShopId2 = selectedDTO.RecommandShopId2;
        item.Note = selectedDTO.Note;
        item.OrderPrice = selectedDTO.OrderPrice;
      }
      await context.SaveChangesAsync(null);
      return true;
    }

    public async Task<StockRequireSummaryDTO?> FindRequireSummary(long productionId)
    {
      return await QueryRequireSummary(null, false).Where(b => b.ProductionId == productionId).FirstOrDefaultAsync();
    }
  }
}
