using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.DTOs;
using ReheeCmf.Reflects.ReflectPools;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class OrderPageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }

    public Type? EntityType { get; set; }

    public List<OrderTrackProduction>? Productions { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    public async Task RefreshTable()
    {

      Productions = await Context!.Query<OrderTrackProduction>(true).ToListAsync();
      StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {

      }
    }

    public async Task CreateProduction(long? id = null)
    {
      //nm.NavigateTo("/entity/order/-1");

      //return;
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = null,
            ["OnSave"] = onsave
          });
      var dotion = new DialogOption()
      {
        Title = id == null ? "new order" : "edit order",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          if (onsave.OnSaveFunc != null)
          {
            return await onsave.OnSaveFunc();
          }
          return true;
        }
      };
      await dialogService!.Show(dotion);
    }
    public OrderTrackProduction? Model { get; set; }

  }
}