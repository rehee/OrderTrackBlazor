namespace OrderTrackBlazor.Entities
{
  public class OrderTrackCategory : BaseOrderTrackEntity
  {
    public virtual List<OrderTrackProduction>? Productions { get; set; }
  }
}
