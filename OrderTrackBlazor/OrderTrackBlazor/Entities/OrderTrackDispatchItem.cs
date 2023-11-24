using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackDispatchItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackDispatchRecord))]
    public long? DispatchRecordId { get; set; }
    public virtual OrderTrackDispatchRecord? DispatchRecord { get; set; }


    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }

    public int Quantity { get; set; }
  }
}
