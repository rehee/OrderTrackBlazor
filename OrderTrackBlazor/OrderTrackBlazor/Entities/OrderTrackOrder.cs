namespace OrderTrackBlazor.Entities
{
  public class OrderTrackOrder : BaseOrderTrackEntity
  {
    public DateTime? OrderDate { get; set; }
    public string? ShortNote { get; set; }
    public string? Note { get; set; }

    public virtual List<OrderTrackOrderItem>? Items { get; set; }
    public virtual List<OrderTrackDispatchRecord>? DispatchRecords { get; set; }
    public virtual List<OrderTrackPurchaseRecord>? PurchaseRecords { get; set; }
    public virtual List<OrderTrackPackage>? Packages { get; set; }
  }
}
