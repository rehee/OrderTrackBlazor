using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackPurchaseRecord : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackShop))]
    public long? ShopId { get; set; }
    public virtual OrderTrackShop? Shop { get; set; }

    public virtual List<OrderTrackPurchaseItem>? Items { get; set; }
  }
}
