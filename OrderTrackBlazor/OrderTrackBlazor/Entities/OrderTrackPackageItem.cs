using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackPackageItem : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackPackage))]
    public long? PackageId { get; set; }
    public virtual OrderTrackPackage? Package { get; set; }

    [ForeignKey(nameof(OrderTrackProduction))]
    public long? ProductionId { get; set; }
    public virtual OrderTrackProduction? Production { get; set; }
    public int Number { get; set; }
  }
}
