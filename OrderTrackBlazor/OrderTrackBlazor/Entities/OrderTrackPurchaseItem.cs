using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackPurchaseItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackPurchaseRecord))]
    public long? PurchaseRecordId { get; set; }
    public virtual OrderTrackPurchaseRecord? PurchaseRecord { get; set; }
    
    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }

    public int Quantity { get; set; }

  }
}
