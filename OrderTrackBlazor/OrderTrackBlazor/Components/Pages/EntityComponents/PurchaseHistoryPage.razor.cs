using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PurchaseHistoryPageComponent : CBase
  {
    public DateTime? QueryBegin { get; set; } = DateTime.UtcNow.AddMonths(-1);
    public DateTime? QueryEnd { get; set; } = DateTime.UtcNow;
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();
    [Inject]
    public IPurchaseService PurchaseService { get; set; }
    [Inject]
    public ISelectedItemService ItemService { get; set; }
    [Inject]
    public IStockService StockService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }

    public IEnumerable<PurchaseDTO>? Purchases { get; set; } = Enumerable.Empty<PurchaseDTO>();
    public async Task Refresh()
    {
      var begin = QueryBegin.HasValue ? (DateTime?)QueryBegin.Value.Date : null;
      var end = QueryEnd.HasValue ? (DateTime?)QueryEnd.Value.Date : null;
      Purchases = await PurchaseService.Query()
        .Where(b => b.PurchaseDate >= begin && b.PurchaseDate <= end)
        .OrderByDescending(b => b.PurchaseDate)
        .ThenByDescending(b => b.Id)
        .ToArrayAsync();
      Shops = await ItemService.GetEntitySelection<OrderTrackShop>();
      StateHasChanged();
    }
    public async Task EditPurchase(PurchaseDTO dto)
    {
      if (dto.OrderId != null)
      {
        await OrderPurchase(dto.OrderId, dto.Id);
      }
      else
      {
        var queryDto = await StockService.FindStockPurchase(dto.Id ?? 0);
        await TaskStockPurchase(queryDto);
      }
    }
    public async Task OrderPurchase(long? orderId, long? purchaseId)
    {
      var onSave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderPurchaseDetail>(
          new Dictionary<string, object?>()
          {
            ["OrderId"] = orderId,
            ["Id"] = purchaseId,
            ["OnSave"] = onSave,
            ["Shops"] = Shops
          });
      var dotion = new DialogOption()
      {
        Title = $"{(purchaseId == null ? "Create" : "Edit")} Purchase",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onSave.OnSaveFunc != null)
          {
            result = await onSave.OnSaveFunc();
          }
          await Refresh();
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }

    public async Task TaskStockPurchase(StockPurchaseDTO dto)
    {
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<StockPurchaseDetail>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["DTO"] = dto,
            ["Shops"] = Shops,
          });
      var dotion = new DialogOption()
      {
        Title = "edit purchase",
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