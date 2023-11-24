using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.DTOs;
using ReheeCmf.Reflects.ReflectPools;
using static Dropbox.Api.Files.FileCategory;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class OrderPageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }

    public Type? EntityType { get; set; }

    public List<OrderDTO>? Orders { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    [Inject]
    public IOrderService? orderService { get; set; }
    public async Task RefreshTable()
    {

      Orders = await orderService.Query().OrderBy(b => b.OrderDate).ToListAsync();
      StateHasChanged();
    }


    public async Task CreateProduction(long? id = null)
    {
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
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
            await onsave.OnSaveFunc();
          }
          await RefreshTable();
          return true;
        }
      };
      await dialogService!.Show(dotion);
    }

    public async Task DispatchList(long? id = null)
    {
      var comp = BootstrapDynamicComponent.CreateComponent<DispatchPage>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
          });
      var dotion = new DialogOption()
      {
        Title = "dispatch",
        Size = Size.ExtraLarge,
        Component = comp,
      };
      await dialogService!.Show(dotion);
    }

    public OrderTrackProduction? Model { get; set; }

  }
}