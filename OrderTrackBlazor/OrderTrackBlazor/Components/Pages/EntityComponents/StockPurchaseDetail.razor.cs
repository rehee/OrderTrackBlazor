using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
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
    public IProductionService productionService { get; set; }

    [Inject]
    public ISelectedItemService selectedItemService { get; set; }

    [Inject]
    public IStockService Service { get; set; }

    public IEnumerable<SelectedItem> ProductionSelections { get; set; } = Enumerable.Empty<SelectedItem>();
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
    public async Task AddProduction()
    {
      await dialogService.ShowComponent<StockPurchaseProduction>(
        new Dictionary<string, object?>
        {
          ["ProductionSelections"] = ProductionSelections,
          ["DTO"] = DTO,
        },
        "",
        true,
        async save => StateHasChanged()
        );
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (DTO.ShopId != null)
      {
        SelectShopItem = Shops.FirstOrDefault(b => b.Value == DTO.ShopId?.ToString());
      }
      ProductionSelections = await selectedItemService.GetEntitySelection<OrderTrackProduction>();
    }


    public IEnumerable<StockRequireSummaryDTO> DisplayDTOs
    {
      get
      {
        if (DTO.PurchaseId > 0)
        {
          return (DTO.RequestInput.Concat(DTO.Request)).OrderByDescending(b => b.Number.HasValue && b.Number != 0).ThenBy(b => b.ProductionName);
        }
        return (DTO.RequestInput.Concat(DTO.Request)).OrderByDescending(b => b.RecommandShops.Contains(SelectShopItem?.Text));
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