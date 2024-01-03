using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackStockDispatchPackage : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackStockDispatch))]
    public long? OrderTrackStockDispatchId { get; set; }
    public virtual OrderTrackStockDispatch? Dispatch { get; set; }
    public decimal PackagePrice { get; set; }
    public string? BriefDiscribtion { get; set; }
    public string? Discribtion { get; set; }
    public int? Number { get; set; }
    [ForeignKey(nameof(OrderTrackStockDispatch))]
    public long? PackageSizeId { get; set; }
    
    public decimal PackageWeight { get; set; }
    public virtual OrderTrackPackageSize? PackageSize { get; set; }
    public virtual List<OrderTrackStockDispatchPackageItem>? Items { get; set; }
    public bool? Confirmed { get; set; }
    public virtual List<OrderTrackDispatchItem>? OrderItems { get; set; }
  }
}
