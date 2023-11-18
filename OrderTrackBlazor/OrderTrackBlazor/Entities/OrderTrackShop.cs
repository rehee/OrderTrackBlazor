

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackShop : BaseOrderTrackEntity
  {
    public EnumShop ShopType { get; set; }
    public string? PostCode { get; set; }
    public string? Note { get; set; }
  }
}
