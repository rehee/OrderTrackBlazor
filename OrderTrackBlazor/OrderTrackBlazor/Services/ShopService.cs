using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class ShopService : IShopService
  {
    private readonly IContext context;

    public ShopService(IContext context)
    {
      this.context = context;
    }
    public async Task<IEnumerable<SelectedItem>> GetShopSelected()
    {
      var query =
        from shop in context.Query<OrderTrackShop>(true).OrderBy(b => b.DisplayOrder).ThenBy(b => b.Id)
        select new SelectedItem
        {
          Text = shop.Name == null ? "" : shop.Name,
          Value = shop.Id.ToString()
        };
      var result = new SelectedItem[] { new SelectedItem("", "select") };
      return result.Concat(await query.ToArrayAsync());
    }
  }
}
