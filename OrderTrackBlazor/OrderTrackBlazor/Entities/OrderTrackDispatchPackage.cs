using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackDispatchPackage : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackShop))]
    public long? PackageId { get; set; }
    public virtual OrderTrackPackage? Package { get; set; }
    [ForeignKey(nameof(OrderTrackDispatchRecord))]
    public long? RecordId { get; set; }
    public virtual OrderTrackDispatchRecord? Record { get; set; }
    public int? Number { get; set; }
    public string? BriefDiscribtion { get; set; }
    public string? Discribtion { get; set; }
  }
}
