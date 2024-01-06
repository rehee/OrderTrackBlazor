
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
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
  public class StockDispatchComponent : CBase
  {

    [Inject]
    public IStockDispatchService? stockDispatchService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }
    public IEnumerable<StockDispatchDTO> DispatchDTO { get; set; } = Enumerable.Empty<StockDispatchDTO>();
    public async Task Refresh()
    {
      DispatchDTO = await stockDispatchService.StockDispatchQuery().ToArrayAsync();
      StateHasChanged();
    }


    public async Task OpenStockDispatch(long? id = null)
    {
      await dialogService.ShowComponent<StockDispatchPage>(
        new Dictionary<string, object?>
        {
          ["Id"] = id
        },
        "",
        true,
        async save => await Refresh()
        );
    }

    public async Task ViewStockDispatch(long id)
    {

      await dialogService.ShowComponent<ViewStockDispatchPage>(
        new Dictionary<string, object?>
        {
          ["Id"] = id
        });
    }
  }

}