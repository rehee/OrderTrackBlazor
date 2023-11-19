using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackBlazor.DTOs
{
  public class ProductionDTO : DTOBase
  {
    [AutoGenerateColumn(Ignore = true)]
    public override long? Id { get; set; }
    [Display(Name = "产品")]
    public string? ProductionName { get; set; }
  }
}
