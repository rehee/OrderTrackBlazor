
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Consts;
using System.Text;
using System.Web;


namespace OrderTrackBlazor.Components.Pages
{
  public class HomeComponent : CBase
  {
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    public List<OrderTrackShop> Shops { get; set; } = new List<OrderTrackShop>();

    public List<PurchaseDTO> Purchase { get; set; } = new List<PurchaseDTO>();
    [Inject]
    public IPurchaseService? purchaseService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();
      Shops = await Context.Query<OrderTrackShop>(true).ToListAsync();
      await Refresh();
    }

    public async Task Refresh()
    {
      Purchase = await purchaseService.Query().OrderByDescending(b => b.PurchaseDate).ToListAsync();
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
        }
      };
      await dialogService!.Show(dotion);
    }


  }

}