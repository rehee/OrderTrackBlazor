using Microsoft.EntityFrameworkCore;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackOrderItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackOrder))]
    public long? OrderTrackOrderId { get; set; }
    public virtual OrderTrackOrder? Order { get; set; }


    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }
    public decimal? OrderPrice { get; set; }
    public int Quantity { get; set; }
    public virtual List<OrderTrackDispatchItem>? DispatchItems { get; set; }

    [ForeignKey(nameof(RecommendShopId))]
    public long? RecommendShopId { get; set; }

    [ForeignKey(nameof(RecommendShopId))]
    public virtual OrderTrackShop RecommendShop { get; set; }

    [ForeignKey(nameof(RecommendShopId2))]
    public long? RecommendShopId2 { get; set; }

    [ForeignKey(nameof(RecommendShopId2))]
    public virtual OrderTrackShop RecommendShop2 { get; set; }
    public string? Note { get; set; }
    public int OverDeliveredNumber { get; set; }
    public async Task OverDeliveredCheck(IContext context)
    {
      var dispatched = await
        context.Query<OrderTrackDispatchItem>(true)
        .Where(b => b.OrderProductionId == Id)
        .Where(b => b.DispatchRecord == null || b.DispatchRecord.Status != EnumDispatchStatus.Error)
        .SumAsync(b => b.Quantity + b.PackageQuantity);
      OverDeliveredNumber = Quantity < dispatched ? Quantity : dispatched;
    }
  }

  [EntityChangeTracker<OrderTrackOrderItem>]
  public class OrderTrackOrderItemHandler : OrderTrackEntityHandler<OrderTrackOrderItem>
  {
    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      await entity.OverDeliveredCheck(context);
    }
    public override async Task AfterUpdateAsync(CancellationToken ct = default)
    {
      await base.AfterUpdateAsync(ct);
      await entity.OverDeliveredCheck(context);
    }
  }
}


