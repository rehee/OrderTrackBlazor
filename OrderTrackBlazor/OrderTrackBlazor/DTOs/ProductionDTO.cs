using BootstrapBlazor.Components;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackBlazor.DTOs
{
  public class ProductionDTO : DTOBase
  {
    [Display(Name = "产品")]
    public string? ProductionName { get; set; }

    [Display(Name = "原始价格")]

    public string? OriginalPrice { get; set; }


    [AutoGenerateColumn(Ignore = true)]
    public decimal? Price
    {
      get
      {
        if (decimal.TryParse(OriginalPrice, out var price))
        {
          return price;
        }
        return null;
      }
    }

  }
}
