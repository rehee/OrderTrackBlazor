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
      await dialogService.ShowComponent<ProductionDetail>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,

        },
       id == null ? "new Product" : "edit Product",
       true,
       async save => await RefreshTable()
       );
    }
    public OrderTrackProduction? Model { get; set; }


  }
}