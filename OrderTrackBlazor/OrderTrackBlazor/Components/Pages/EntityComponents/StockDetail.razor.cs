using Microsoft.AspNetCore.Components;
using OrderTrackBlazor.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class StockDetailComponent : CBase
  {
    [Parameter]
    public string? Name { get; set; }
    [Parameter]
    public long ProductionId { get; set; }
    [Inject]
    public IStockService? stockService { get; set; }

    public List<StockListDTO> StockLists { get; set; } = new List<StockListDTO>();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      StockLists = stockService.QueryDetail()
        .Where(b => b.Id == ProductionId)
        .OrderByDescending(b => b.Date).ThenByDescending(b => b.CreateDate)
        .ToList();
      StateHasChanged();
    }
  }
}