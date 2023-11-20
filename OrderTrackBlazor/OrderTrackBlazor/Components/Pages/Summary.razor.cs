
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Data;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages
{
  public class SummaryComponent : CBase
  {
    //public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    //public List<OrderTrackShop> Shops { get; set; } = new List<OrderTrackShop>();

    //public List<PurchaseDTO> Purchase { get; set; } = new List<PurchaseDTO>();
    [Inject]
    public ISummaryService? summaryService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      //Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();
      //Shops = await Context.Query<OrderTrackShop>(true).ToListAsync();
      //await Refresh();
      SummaryDTOs = await summaryService.Query().OrderBy(b => b.OrderDate).ToListAsync();
    }
    public List<SummaryDTO> SummaryDTOs { get; set; } = new List<SummaryDTO>();
    public async Task Refresh()
    {
      //Purchase = await purchaseService.Query().OrderByDescending(b => b.PurchaseDate).ToListAsync();
      //StateHasChanged();
    }




  }

}