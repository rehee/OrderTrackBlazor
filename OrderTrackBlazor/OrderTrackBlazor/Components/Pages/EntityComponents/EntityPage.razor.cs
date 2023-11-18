using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Reflects.ReflectPools;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class EntityPageComponent : CBase
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
      if (Shops?.Any() != true)
      {
        Array enumValues = Enum.GetValues(typeof(EnumShop));
        foreach (var e in enumValues)
        {
          if (e is EnumShop ev)
          {
            await Context.AddAsync<OrderTrackShop>(new OrderTrackShop()
            {
              ShopType = ev
            }, CancellationToken.None);
          }
        }
        Context.SaveChanges(null);
      }
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