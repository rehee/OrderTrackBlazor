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
  }
}
