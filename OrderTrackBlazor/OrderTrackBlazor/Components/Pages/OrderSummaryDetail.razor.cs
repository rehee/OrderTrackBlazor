using BootstrapBlazor.Components;
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
    public List<OrderTrackShop> Shops { get; set; } = new List<OrderTrackShop>();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshPage();
    }
    public async Task RefreshPage()
    {
      Shops = Context.Query<OrderTrackShop>(true).ToList();
      Summary = await summaryService.Query().Where(b => b.OrderId == Id).FirstOrDefaultAsync();
      OrderPurchaseSummaryDTO = summaryService.GetOrderSummary(Id).OrderByDescending(b => b.PurchaseDate).ThenByDescending(b => b.CreateDate).ToList();
      StateHasChanged();
    }
    public List<OrderPurchaseSummaryDTO> OrderPurchaseSummaryDTO { get; set; } = new List<OrderPurchaseSummaryDTO>();
    public async Task Shopping(long? id = null)
    {
      var onSave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderPurchaseDetail>(
          new Dictionary<string, object?>()
          {
            ["OrderId"] = Id,
            ["Id"] = id,
            ["OnSave"] = onSave,
            ["Shops"] = Shops
          });
      var dotion = new DialogOption()
      {
        Title = "Create Dispatch",
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
          await RefreshPage();
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }
  }
}