using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<ProductionDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
            ["OnSave"] = onsave,
          });
      var dotion = new DialogOption()
      {
        IsScrolling = true,
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
            await Refresh(true);
          }
          return result;
        }
      };
      await dialogService!.Show(dotion);
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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<StockPurchaseDetail>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["DTO"] = new StockPurchaseDTO(DTOs, 0, DateTime.UtcNow.Date, null, null),
            ["Shops"] = Shops,
            ["SelectShopItem"] = SelectShopItem,
          });
      var dotion = new DialogOption()
      {
        IsScrolling = true,
        Title = purchaseId == null ? "add package" : "edit package",
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

          }
          await Refresh();
          return result;
        }
      };
      await dialogService!.Show(dotion);
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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderItemPage>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["DTOs"] = records.Items.OrderByDescending(b => b.OrderDate),
            ["Shops"] = Shops
          });
      var dotion = new DialogOption()
      {
        IsScrolling = true,
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

          }
          await Refresh();
          return result;
        }
      };
      await dialogService!.Show(dotion);

    }
  }
}