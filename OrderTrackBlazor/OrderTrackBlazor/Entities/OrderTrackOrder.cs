namespace OrderTrackBlazor.Entities
{
  public class OrderTrackOrder : BaseOrderTrackEntity
  {
    public DateTime? OrderDate { get; set; }

    public virtual List<OrderTrackOrderItem>? Items { get; set; }
    public virtual List<OrderTrackDispatchRecord>? DispatchRecords { get; set; }
  }
}
