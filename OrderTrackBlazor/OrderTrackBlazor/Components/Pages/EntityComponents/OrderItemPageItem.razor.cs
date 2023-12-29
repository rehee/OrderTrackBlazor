using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class OrderItemPageItemComponent : CBase
  {
    [Parameter]
    public StockRequireDTO DTO { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();

    public SelectedItem? SelectShopItem { get; set; }
    public SelectedItem? SelectShopItem2 { get; set; }
    public async Task OnItemChanged(SelectedItem item)
    {
      await Task.CompletedTask;
      if (DTO == null)
      {
        return;
      }

      if (long.TryParse(item?.Value, out var shopId))
      {
        DTO.RecommandShopId = shopId;
      }
      else
      {
        DTO.RecommandShopId = null;
      }

    }
    public async Task OnItemChanged2(SelectedItem item)
    {
      await Task.CompletedTask;
      if (DTO == null)
      {
        return;
      }

      if (long.TryParse(item?.Value, out var shopId))
      {
        DTO.RecommandShopId2 = shopId;
      }
      else
      {
        DTO.RecommandShopId2 = null;
      }

    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      SelectShopItem = Shops.FirstOrDefault(b => b.Value == DTO?.RecommandShopId?.ToString());
      SelectShopItem2 = Shops.FirstOrDefault(b => b.Value == DTO?.RecommandShopId2?.ToString());
    }

  }
}