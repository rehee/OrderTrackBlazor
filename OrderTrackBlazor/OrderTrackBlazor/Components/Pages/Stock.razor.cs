
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Consts;
using OrderTrackBlazor.Data;
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


    public async Task NormalShowDialog(long? productionId)
    {

      var comp = BootstrapDynamicComponent.CreateComponent<StockDetail>(
          new Dictionary<string, object?>()
          {
            ["ProductionId"] = productionId,
            ["Name"] = SummaryDTOs.Where(b => b.Id == productionId).Select(b => b.Name).FirstOrDefault()
          });
      string chineseString = DefaultValues.PTitle;
      System.Console.WriteLine(DefaultValues.PTitle);
      byte[] utf8Bytes = Encoding.UTF8.GetBytes(chineseString);
      string decodedString = Encoding.UTF8.GetString(utf8Bytes);
      var dotion = new DialogOption()
      {
        IsScrolling = true,
        Size = Size.ExtraLarge,
        Component = comp,
      };
      await dialogService!.Show(dotion);
    }


  }

}