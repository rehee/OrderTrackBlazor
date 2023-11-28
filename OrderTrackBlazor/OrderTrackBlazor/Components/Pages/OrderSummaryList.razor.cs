using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages
{
  public class OrderSummaryListComponent : CBase
  {
    [Inject]
    public ISummaryService? summaryService { get; set; }

    [Inject]
    public NavigationManager nav { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      //Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();
      //Shops = await Context.Query<OrderTrackShop>(true).ToListAsync();
      //await Refresh();
      SummaryDTOs = await summaryService.Query().OrderByDescending(b => b.OrderDate).ThenByDescending(b => b.OrderCreateDate).ToListAsync();
    }
    public List<SummaryDTO> SummaryDTOs { get; set; } = new List<SummaryDTO>();
    public async Task Refresh()
    {
      //Purchase = await purchaseService.Query().OrderByDescending(b => b.PurchaseDate).ToListAsync();
      //StateHasChanged();
    }
  }
}