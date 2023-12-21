using Microsoft.EntityFrameworkCore;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackPurchaseRecord : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackShop))]
    public long? ShopId { get; set; }
    public virtual OrderTrackShop? Shop { get; set; }

    public DateTime? PurchaseDate { get; set; }
    public virtual List<OrderTrackPurchaseItem>? Items { get; set; }

    [ForeignKey(nameof(OrderTrackOrder))]
    public long? OrderId { get; set; }
    public virtual OrderTrackOrder? Order { get; set; }

    public decimal? Price { get; set; }
  }
  [EntityChangeTracker<OrderTrackPurchaseRecord>]
  public class OrderTrackPurchaseRecordHandler : OrderTrackEntityHandler<OrderTrackPurchaseRecord>
  {
    public override async Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      await base.BeforeDeleteAsync(ct);
      var items = await context.Query<OrderTrackPurchaseItem>(false).Where(b => b.PurchaseRecordId == entity.Id).ToArrayAsync();
      foreach (var item in items)
      {
        context.Delete(item);
      }
    }
  }
}
