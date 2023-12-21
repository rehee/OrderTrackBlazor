using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using static Dropbox.Api.Files.ListRevisionsMode;

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
        return DTOs.OrderByDescending(b => b.RecommandShops.Contains(SelectShopItem?.Text));
      }
    }
    public async Task Refresh()
    {
      SelectShopItem = null;
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
  }
}