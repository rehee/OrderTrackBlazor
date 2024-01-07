using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackBlazor.DTOs
{
  public class ProductionDTO : DTOBase
  {
    [Display(Name = "产品")]
    public string? ProductionName { get; set; }

    [Display(Name = "原始价格")]

    public decimal? OriginalPrice { get; set; }

    public decimal? WeightGrem { get; set; }
    public long? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? ExtendUrl { get; set; }

    public long NewId { get; set; }

    public string? Attachment { get; set; }

  }
}
