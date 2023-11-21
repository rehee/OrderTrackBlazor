
namespace OrderTrackBlazor.DTOs
{
  public class PurchaseDTO : DTOBase, IWithOrderProductionsDTO
  {
    public DateTime? PurchaseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? ShopId { get; set; }
    public List<OrderProductionDTO>? Productions { get; set; }
  }

  public class PurchaseListDTO : DTOBase
  {
    public DateTime? PurchaseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? ShopId { get; set; }
    public long? ItemId { get; set; }
    public long? ProductionId { get; set; }
    public int Quantity { get; set; }
  }
}
