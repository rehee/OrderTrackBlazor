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

      Orders = await orderService.Query().OrderByDescending(b => b.OrderDate).ThenByDescending(b => b.Id).ToListAsync();
      StateHasChanged();
    }


    public async Task CreateProduction(long? id = null)
    {
      await dialogService.ShowComponent<OrderDetail>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,
        },
        id == null ? "new order" : "edit order",
        true,
        async save => await RefreshTable()
        );
    }

    public async Task DispatchList(long? id = null)
    {
      await dialogService.ShowComponent<DispatchPage>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,
        },
        "dispatch"
        );
    }
    public async Task PackageList(long? id = null)
    {
      await dialogService.ShowComponent<PackagePage>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,
        },
        "Packages"
       );
    }
    public OrderTrackProduction? Model { get; set; }

  }
}