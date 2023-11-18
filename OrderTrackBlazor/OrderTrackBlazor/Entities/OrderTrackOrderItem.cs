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

    public int Quantity { get; set; }
  }
}
