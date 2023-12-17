using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackPackage : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackOrder))]
    public long? OrderId { get; set; }
    public virtual OrderTrackOrder? Order { get; set; }
    [ForeignKey(nameof(OrderTrackPackageSize))]
    public long? SizeId { get; set; }
    public virtual OrderTrackPackageSize? Size { get; set; }
    public decimal? Weight { get; set; }
    public string? BriefDiscribtion { get; set; }
    public string? Discribtion { get; set; }
    public virtual List<OrderTrackPackageItem>? Items { get; set; }
    public bool? Confirmed { get; set; }

  }
}
