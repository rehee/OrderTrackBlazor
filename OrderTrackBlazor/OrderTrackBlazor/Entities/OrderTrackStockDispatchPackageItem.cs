using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackStockDispatchPackageItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackStockDispatchPackage))]
    public long? PackageId { get; set; }
    public virtual OrderTrackStockDispatchPackage? Package { get; set; }
    public int Number { get; set; }
    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }
  }
}
