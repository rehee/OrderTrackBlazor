
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Consts;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.Helpers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages
{
  public class StockComponent : CBase
  {
    //public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    //public List<OrderTrackShop> Shops { get; set; } = new List<OrderTrackShop>();

    //public List<PurchaseDTO> Purchase { get; set; } = new List<PurchaseDTO>();
    [Inject]
    public IStockService? stockService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();

    }
    public List<StockSummaryDTO> SummaryDTOs { get; set; } = new List<StockSummaryDTO>();
    public async Task Refresh()
    {
      SummaryDTOs = await stockService.QuerySummary().OrderByDescending(b => b.CurrentStock).ThenBy(b => b.Id).ToListAsync();
      StateHasChanged();
    }
    [Inject]
    public IJSRuntime js { get; set; }
    public int? DisplayHours { get; set; } = 4;
    public async Task ShowAvaliable(bool? showAvaliable = true, bool stillOpen = false)
    {
      //await dialogService.ShowComponent<AvaliableStock>(
      //  null,
      // "");
      if (showAvaliable == true)
      {
        AvaliableStock.AvaliableUntil = DateTime.UtcNow.AddHours(DisplayHours ?? 4);
        //await js.InvokeVoidAsync("open", "avaliable", "_blank");
        nm.NavigateTo("avaliable", false);

      }
      if (showAvaliable == false)
      {
        AvaliableStock.AvaliableUntil = null;
      }
      if (stillOpen)
      {
        nm.NavigateTo("avaliable", false);
      }
    }
    public async Task NormalShowDialog(long? productionId)
    {
      await dialogService.ShowComponent<StockDetail>(
        new Dictionary<string, object?>
        {
          ["ProductionId"] = productionId,
          ["Name"] = SummaryDTOs.Where(b => b.Id == productionId).Select(b => b.Name).FirstOrDefault()
        }, ""
        );
    }

    public async Task Maintain(long? id = null)
    {
      await dialogService.ShowComponent<ProductionDetail>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,

        },
       id == null ? "new Product" : "edit Product",
       true,
       async save => await Refresh()
       );
    }
  }

}