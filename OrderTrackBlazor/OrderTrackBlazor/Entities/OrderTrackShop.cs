

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackShop : BaseOrderTrackEntity
  {
    public string? ShopName { get; set; }
    public string? PostCode { get; set; }
    public string? Note { get; set; }
    public int DisplayOrder { get; set; }
  }
}
