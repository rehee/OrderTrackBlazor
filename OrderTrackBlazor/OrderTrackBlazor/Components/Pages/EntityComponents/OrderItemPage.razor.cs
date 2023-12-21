using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class OrderItemPageComponent : CBase
  {
    [Parameter]
    public IEnumerable<StockRequireDTO> DTOs { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();

    [Inject]
    public IStockService StockService { get; set; }
    public override async Task<bool> SaveFunction()
    {
      return await StockService.UpdateStockOrderItem(DTOs);
    }
  }
}