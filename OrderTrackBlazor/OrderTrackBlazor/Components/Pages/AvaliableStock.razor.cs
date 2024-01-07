using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages
{
  public class AvaliableStockComponent : CBase
  {
    [Inject]
    public IStockService? stockService { get; set; }
    public IEnumerable<StockSummaryDTO> SummaryDTOs { get; set; } = Enumerable.Empty<StockSummaryDTO>();
    public static DateTime? AvaliableUntil { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (AvaliableUntil == null || AvaliableUntil.Value < DateTime.UtcNow)
      {
        return;
      }
      SummaryDTOs = await stockService.QuerySummary().Where(b => b.CurrentStock > 0)
        .OrderBy(b => b.Name).ToArrayAsync();
    }
  }
}