using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.DTOs;
using ReheeCmf.Reflects.ReflectPools;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ProductionPageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }
    public Type? EntityType { get; set; }
    public IEnumerable<ProductionDTO>? Productions { get; set; }

    [Inject]
    public IProductionService? ProductionService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    public async Task RefreshTable()
    {

      Productions = await ProductionService.GetAllProductions();
      StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);

    }

    public async Task CreateProduction(long? id = null)
    {
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<ProductionDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
            ["OnSave"] = onsave,
          });
      var dotion = new DialogOption()
      {
        Title = $"{(id == null ? "create" : "edit")} Product",
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
            await RefreshTable();
          }
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }
    public OrderTrackProduction? Model { get; set; }


  }
}