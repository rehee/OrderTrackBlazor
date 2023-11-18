using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackDispatchRecord : BaseOrderTrackEntity
  {
    public DateTime? DispatchDate { get; set; }
    [ForeignKey(nameof(OrderTrackOrder))]
    public long? OrderTrackOrderId { get; set; }
    public virtual OrderTrackOrder? Order { get; set; }

    public virtual List<OrderTrackDispatchItem>? Items { get; set; }
  }
}
