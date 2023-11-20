namespace OrderTrackBlazor.Entities
{
  public class OrderTrackProduction : BaseOrderTrackEntity
  {
    public string? Name { get; set; }
    public virtual List<OrderTrackPurchaseItem>? PurchaseItems { get; set; }
  }
}
