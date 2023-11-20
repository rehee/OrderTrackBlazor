
namespace OrderTrackBlazor.DTOs
{
  public class PurchaseDTO : DTOBase, IWithOrderProductionsDTO
  {
    public DateTime? PurchaseDate { get; set; }
    public long? ShopId { get; set; }
    public List<OrderProductionDTO>? Productions { get; set; }

  }
}
