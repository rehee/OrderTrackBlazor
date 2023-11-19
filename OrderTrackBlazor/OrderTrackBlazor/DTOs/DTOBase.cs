using BootstrapBlazor.Components;

namespace OrderTrackBlazor.DTOs
{
  public abstract class DTOBase
  {
    [AutoGenerateColumn(Ignore = true)]
    public virtual long? Id { get; set; }
    [AutoGenerateColumn(Ignore = true)]
    public bool IsCreate => Id.HasValue != true || Id <= 0;
  }
}
