namespace OrderTrackBlazor.Entities
{
  public class OrderTrackProduction : BaseOrderTrackEntity
  {
    public decimal? OriginalPrice { get; set; }
    public virtual List<OrderTrackPurchaseItem>? PurchaseItems { get; set; }
    public virtual List<OrderTrackDispatchItem>? DispatchItems { get; set; }
    public virtual List<OrderTrackOrderItem>? OrderItems { get; set; }
  }
}
