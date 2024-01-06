using BootstrapBlazor.Components;
using Google.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;

namespace OrderTrackBlazor.Components.Pages
{
  public class OrderSummaryDetailComponent : CBase
  {
    [Parameter]
    public long Id { get; set; }
    [Inject]
    public ISummaryService? summaryService { get; set; }
    public SummaryDTO? Summary { get; set; }
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshPage();
    }
    [Inject]
    public IShopService? shopService { get; set; }
    public async Task RefreshPage()
    {
      Shops = await shopService.GetShopSelected();
      Summary = await summaryService.Query().Where(b => b.OrderId == Id).FirstOrDefaultAsync();
      OrderPurchaseSummaryDTO = summaryService.GetOrderSummary(Id).OrderByDescending(b => b.PurchaseDate).ThenByDescending(b => b.CreateDate).ToList();
      StateHasChanged();
    }
    public List<OrderPurchaseSummaryDTO> OrderPurchaseSummaryDTO { get; set; } = new List<OrderPurchaseSummaryDTO>();
    public async Task Shopping(long? id = null)
    {
      await dialogService.ShowComponent<OrderPurchaseDetail>(
        new Dictionary<string, object?>()
        {
          ["OrderId"] = Id,
          ["Id"] = id,
          ["Shops"] = Shops
        },
       id == null ? "Create Purchase" : "Edit Purchase",
       true,
       async save => await RefreshPage()
       );
    }
  }
}