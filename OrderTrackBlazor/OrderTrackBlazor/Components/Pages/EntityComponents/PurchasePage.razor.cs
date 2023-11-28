using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Consts;
using System.Text;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PurchasePageComponent : CBase
  {
    [Parameter]
    public long? ProductionId { get; set; }
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();

    public List<PurchaseDTO> Purchase { get; set; } = new List<PurchaseDTO>();
    public List<PurchaseListDTO> PurchaseItemList { get; set; } = new List<PurchaseListDTO>();
    [Inject]
    public IPurchaseService? purchaseService { get; set; }
    [Inject]
    public IShopService? shopService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();

      Shops = await shopService.GetShopSelected();
      await Refresh();
    }

    public async Task Refresh()
    {
      Purchase = await
        (ProductionId.HasValue ? purchaseService.Query().Where(b => b.Productions.Any(b => b.ProductionId == ProductionId)) : purchaseService.Query())
        .OrderByDescending(b => b.PurchaseDate).ThenByDescending(b => b.CreateDate).ToListAsync();
      foreach (var p in Purchase)
      {
        foreach (var item in p.Productions)
        {
          item.Parent = p;
        }
      }
      PurchaseItemList.Clear();
      foreach (var p in Purchase)
      {
        foreach (var item in (p.Productions ?? new List<OrderProductionDTO>()).OrderByDescending(b => b.CreateDate))
        {
          if (ProductionId.HasValue && item.ProductionId != ProductionId)
          {
            continue;
          }
          PurchaseItemList.Add(
            new PurchaseListDTO
            {
              Id = p.Id,
              ItemId = item.Id,
              ProductionId = item.ProductionId,
              PurchaseDate = p.PurchaseDate,
              CreateDate = item.CreateDate,
              ShopId = p.ShopId,
              Quantity = item.Quantity,
            });
        }
      }
      StateHasChanged();
    }

    public async Task NormalShowDialog(PurchaseDTO? dto = null)
    {
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<PurchaseDetail>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["Productions"] = Productions,
            ["Shops"] = Shops,
            ["DTO"] = dto
          });
      string chineseString = DefaultValues.PTitle;
      System.Console.WriteLine(DefaultValues.PTitle);
      byte[] utf8Bytes = Encoding.UTF8.GetBytes(chineseString);
      string decodedString = Encoding.UTF8.GetString(utf8Bytes);
      var dotion = new DialogOption()
      {
        Title = decodedString,
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
          await Refresh();
          return result;
        },
        OnCloseAsync = async () =>
        {

          System.Console.WriteLine("Close detected 2");
        },
      };
      await dialogService!.Show(dotion);
    }

  }
}
