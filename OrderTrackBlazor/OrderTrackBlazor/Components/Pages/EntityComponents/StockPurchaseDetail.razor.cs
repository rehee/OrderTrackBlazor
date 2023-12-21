using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class StockPurchaseDetailComponent : CBase
  {
    [Parameter]
    public StockPurchaseDTO DTO { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();
    [Parameter]
    public SelectedItem? SelectShopItem { get; set; }
    public DateTime? PurchaseDate { get; set; } = DateTime.UtcNow.Date;
    public decimal? Price { get; set; }
    [Inject]
    public IStockService Service { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      foreach (var dto in DTO.Request)
      {
        dto.SelectedShop = item.Text;
      }
      if (long.TryParse(item?.Value, out var shopId))
      {
        DTO.ShopId = shopId;
      }
      else
      {
        DTO.ShopId = null;
      }
      return Task.CompletedTask;
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (DTO.ShopId != null)
      {
        SelectShopItem = Shops.FirstOrDefault(b => b.Value == DTO.ShopId?.ToString());
      }

    }


    public IEnumerable<StockRequireSummaryDTO> DisplayDTOs
    {
      get
      {
        if (DTO.PurchaseId > 0)
        {
          return DTO.Request;
        }
        return DTO.Request.OrderByDescending(b => b.RecommandShops.Contains(SelectShopItem?.Text));
      }
    }

    public override async Task<bool> SaveFunction()
    {
      if (DTO.PurchaseId <= 0)
      {
        return await Service.CreateStockPurchase(DTO);
      }
      else
      {
        return await Service.UpdateStockPurchase(DTO);
      }
    }
  }
}