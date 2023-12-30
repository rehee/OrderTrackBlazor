
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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<StockDispatchPage>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
            ["OnSave"] = onsave
            
          });
      var dotion = new DialogOption()
      {
        IsScrolling = true,
        Title = "",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onsave.OnSaveFunc != null)
          {
            result = await onsave.OnSaveFunc();
          }
          if (result)
          {
            await Refresh();
          }
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }

  }

}