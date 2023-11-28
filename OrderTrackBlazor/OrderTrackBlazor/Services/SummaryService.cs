
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class SummaryService : ISummaryService
  {
    private readonly IContext context;

    public SummaryService(IContext context)
    {
      this.context = context;
    }

    public async Task<bool> CreateOrderPurchase(OrderPurchaseSummaryDTO dto)
    {
      var order = context.Query<OrderTrackOrder>(true).Where(b => b.Id == dto.OrderId).FirstOrDefault();
      if (order == null || dto?.Items?.Any() != true || dto?.Items?.All(b => b.Quantity == 0) == true)
      {
        return true;
      }
      var record = new OrderTrackPurchaseRecord
      {
        OrderId = order.Id,
        PurchaseDate = dto.PurchaseDate?.Date,
        Price = dto.Price,
        ShopId = dto.ShopId,
      };
      await context.AddAsync<OrderTrackPurchaseRecord>(record, CancellationToken.None);
      foreach (var orderItem in order.Items.Where(b => b.Quantity != 0) ?? new List<OrderTrackOrderItem>())
      {
        var recordItem = new OrderTrackPurchaseItem
        {
          PurchaseRecord = record,
          PurchaseRecordId = record.Id,
          ProductionId = orderItem.ProductionId,
          Quantity = dto.Items?.Where(b => b.ProductionId == orderItem.ProductionId).Select(b => b.Quantity).FirstOrDefault() ?? 0,
        };
        await context.AddAsync<OrderTrackPurchaseItem>(recordItem, CancellationToken.None);
      }
      context.SaveChanges(null);
      return true;
    }
    public async Task<bool> UpdateOrderPurchase(OrderPurchaseSummaryDTO dto)
    {
      var record = context.Query<OrderTrackPurchaseRecord>(false).Where(b => b.Id == dto.Id).FirstOrDefault();
      if (record == null) return true;
      record.PurchaseDate = dto.PurchaseDate?.Date;
      record.Price = dto.Price;
      record.ShopId = dto.ShopId;
      foreach (var item in dto.EditOrderItem?.ToList() ?? new List<OrderPurchaseItemDTO>())
      {
        if (item.Id > 0)
        {
          var itemRecord = context.Query<OrderTrackPurchaseItem>(false).Where(b => b.Id == item.Id).FirstOrDefault();
          if (itemRecord == null) continue;
          itemRecord.Quantity = item.Quantity;
        }
        else
        {
          if (item.Quantity == 0)
          {
            continue;
          }
          var itemRecord = new OrderTrackPurchaseItem()
          {
            ProductionId = item.ProductionId,
            PurchaseRecordId = record.Id,
            PurchaseRecord = record,
            Quantity = item.Quantity,
          };
          await context.AddAsync<OrderTrackPurchaseItem>(itemRecord, CancellationToken.None);
        }
      }
      context.SaveChanges(null);
      return true;
    }
    public async Task<OrderPurchaseSummaryDTO?> FindOrderPurchaseSummaryDTO(long purchaseId)
    {
      return await GetOrderSummary(0).Where(b => b.Id == purchaseId).FirstOrDefaultAsync();
    }

    public IQueryable<OrderPurchaseSummaryDTO> GetOrderSummary(long orderId)
    {
      return
        from purchase in context.Query<OrderTrackPurchaseRecord>(true).Where(b => orderId <= 0 || b.OrderId == orderId)
        let currentItem = purchase.Items.Select(b => new OrderPurchaseItemDTO
        {
          Id = b.Id,
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production.Name,
          Quantity = b.Quantity,
        })
        let orderItem = purchase.Order.Items.Select(b => new OrderPurchaseItemDTO
        {
          Id = 0,
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production.Name,
          Quantity = 0,
        })

        select new OrderPurchaseSummaryDTO
        {
          OrderId = purchase.OrderId,
          Id = purchase.Id,
          CreateDate = purchase.CreateDate,
          PurchaseDate = purchase.PurchaseDate,
          Price = purchase.Price,
          ShopId = purchase.ShopId,
          Items = currentItem,
          OrderItems = orderItem
        };
    }

    public async Task<OrderPurchaseSummaryDTO> NewOrderPurchaseSummaryDTO(long orderId)
    {
      var order = context.Query<OrderTrackOrder>(true).Where(b => b.Id == orderId).FirstOrDefault();
      if (order == null)
      {
        return new OrderPurchaseSummaryDTO
        {
          Items = new List<OrderPurchaseItemDTO>()
        };
      }
      return new OrderPurchaseSummaryDTO
      {
        OrderId = order.Id,
        PurchaseDate = DateTime.UtcNow,
        Items = order.Items?.Select(b => new OrderPurchaseItemDTO
        {
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name,
        }).OrderBy(b => b.ProductionId).ToList(),
      };
    }

    public IQueryable<SummaryDTO> Query()
    {

      return
        from order in context.Query<OrderTrackOrder>(true)

        let orderItem = order.Items.Select(b => new SummaryProductionDTO
        {
          ProductionId = b.ProductionId,
          Required = b.Quantity,
          Purchased = b.Production.PurchaseItems.Sum(b => b.Quantity) - b.Production.DispatchItems.Where(b => b.DispatchRecord.Status != EnumDispatchStatus.Error).Sum(b => b.Quantity),
          Dispatched = b.Production.DispatchItems.Where(b => b.DispatchRecord.OrderTrackOrderId == order.Id && b.DispatchRecord.Status != EnumDispatchStatus.Error).Sum(b => b.Quantity),
          ProductionName = b.Production.Name,
        })
        select new SummaryDTO
        {
          OrderId = order.Id,
          OrderDate = order.OrderDate,
          OrderCreateDate = order.CreateDate,
          ShortNote = order.ShortNote,
          Productions = orderItem
        };
    }


  }
}
