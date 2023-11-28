using BootstrapBlazor.Components;
using Google.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class OrderPurchaseDetailComponent : CBase
  {
    [Parameter]
    public long? OrderId { get; set; }

    [Parameter]
    public long? Id { get; set; }
    [Parameter]
    public List<OrderTrackShop> Shops { get; set; } = new List<OrderTrackShop>();

    [Inject]
    public ISummaryService? summaryService { get; set; }
    public OrderPurchaseSummaryDTO? Model { get; set; }
    public List<SelectedItem> StatusItem { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedStatusItem { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      if (Model != null && long.TryParse(item.Value, out var result))
      {
        Model.ShopId = result;
      }
      return Task.CompletedTask;
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      StatusItem.Add(new SelectedItem("", "selected"));

      foreach (var shop in Shops)
      {
        StatusItem.Add(new SelectedItem(shop.Id.ToString(), shop.ShopType.ToString()));
      }
      if (Id == null)
      {
        Model = await summaryService!.NewOrderPurchaseSummaryDTO(OrderId ?? 0);
      }
      else
      {
        Model = await summaryService!.FindOrderPurchaseSummaryDTO(Id.Value);
        SelectedStatusItem = StatusItem.Where(b => b.Value == Model?.ShopId?.ToString()).FirstOrDefault();
      }
      StateHasChanged();
    }
    public override async Task<bool> SaveFunction()
    {
      if (Id == null)
      {
        return await summaryService.CreateOrderPurchase(Model);
      }
      else
      {
        return await summaryService.UpdateOrderPurchase(Model);
      }

      return true;
    }
  }
}