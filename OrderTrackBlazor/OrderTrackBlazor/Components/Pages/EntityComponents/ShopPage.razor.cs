using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Reflects.ReflectPools;

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
      
      Shops = await Context!.Query<OrderTrackShop>(true).ToListAsync();
      StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (firstRender)
      {

      }
    }
  }
}