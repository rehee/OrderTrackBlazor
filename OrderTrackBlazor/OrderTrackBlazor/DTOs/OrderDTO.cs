using BootstrapBlazor.Components;

namespace OrderTrackBlazor.DTOs
{
  public class OrderDTO : DTOBase, IWithOrderProductionsDTO
  {
    public DateTime? OrderDate { get; set; }
    public string? ShortNote { get; set; }
    public string? Note { get; set; }
    public List<OrderProductionDTO>? Productions { get; set; }
  }

  public class OrderProductionDTO : DTOBase
  {
    public long? ProductionId { get; set; }
    public long? ParentId { get; set; }
    public IWithOrderProductionsDTO? Parent { get; set; }
    public int Quantity { get; set; }
  }

  public interface IWithOrderProductionsDTO
  {
    public long? Id { get; set; }
    List<OrderProductionDTO>? Productions { get; set; }
  }

}
