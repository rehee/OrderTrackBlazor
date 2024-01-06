using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Consts;
using OrderTrackBlazor.Helpers;
using ReheeCmf.Reflects.ReflectPools;
using System.Text;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ShopPageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }

    public Type? EntityType { get; set; }

    public List<OrderTrackShop>? Shops { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    public async Task RefreshTable()
    {

      Shops = await Context!.Query<OrderTrackShop>(true).OrderBy(b => b.DisplayOrder).ThenBy(b => b.Id).ToListAsync();
      StateHasChanged();
    }
    public async Task NormalShowDialog(long? id = null)
    {
      await dialogService.ShowComponent<ShopDetail>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,
        },
        $"{(id == null ? "Create" : "Edit")}  shop",
        true,
        async (b) => await RefreshTable()
        );
    }
  }
}