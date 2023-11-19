using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackBlazor.DTOs
{
  public class ProductionDTO : DTOBase
  {
    [Display(Name = "产品")]
    public string? ProductionName { get; set; }
  }
}
