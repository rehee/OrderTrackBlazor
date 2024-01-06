using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Helpers;
using System.Diagnostics.CodeAnalysis;
using static Dropbox.Api.Files.ListRevisionsMode;
using static Dropbox.Api.Files.SearchMatchType;

namespace OrderTrackBlazor.Components.Pages
{
  public class StockRequirePageComponent : CBase
  {
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectShopItem { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      foreach (var dto in DTOs)
      {
        dto.SelectedShop = item.Text;
      }
      return Task.CompletedTask;
    }
    [Inject]
    public ISelectedItemService SelectedItemService { get; set; }
    [Inject]
    public IStockService service { get; set; }

    public async Task Copy(string? content)
    {
      await clipboardService.Copy(content);

    }
    public async Task Edit(long? id)
    {
      await dialogService.ShowComponent<ProductionDetail>(
        new Dictionary<string, object?>
        {
          ["Id"] = id
        },
        id == null ? "create Product" : "edit Product",
        true,
        async save => await Refresh(true)
        );
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }
    public IEnumerable<StockRequireSummaryDTO> DTOs { get; set; } = Enumerable.Empty<StockRequireSummaryDTO>();
    public IEnumerable<StockRequireSummaryDTO> DisplayDTOs
    {
      get
      {
        return DTOs.OrderByDescending(b => b.RecommandShops.Contains(SelectShopItem?.Text)).ThenBy(b => b.ProductionName);
      }
    }
    public async Task Refresh(bool keepShop = false)
    {
      if (!keepShop)
      {
        SelectShopItem = null;
      }

      DTOs = await service.QueryRequireSummary().ToListAsync();
      Shops = await SelectedItemService.GetEntitySelection<OrderTrackShop>();
      StateHasChanged();
    }
    public async Task PurchaseDetail(long? purchaseId = null)
    {
      await dialogService.ShowComponent<StockPurchaseDetail>(
        new Dictionary<string, object?>
        {
          ["DTO"] = new StockPurchaseDTO(DTOs, 0, DateTime.UtcNow.Date, null, null),
          ["Shops"] = Shops,
          ["SelectShopItem"] = SelectShopItem,
        },
        purchaseId == null ? "Add Purchase" : "edit Purchase",
        true,
        async save => await Refresh()
        );
    }
    public Task History()
    {
      nm.NavigateTo("purchases", false);
      return Task.CompletedTask;
    }
    public async Task ShowDetail(long? productionId)
    {
      var records = DTOs.FirstOrDefault(b => b.ProductionId == productionId);
      if (records == null || records.Items?.Any() != true)
      {
        return;
      }
      await dialogService.ShowComponent<OrderItemPage>(
        new Dictionary<string, object?>
        {
          ["DTOs"] = records.Items.OrderByDescending(b => b.OrderDate),
          ["Shops"] = Shops
        },
        "",
        true,
        async save => await Refresh()
        );
    }
  }
}