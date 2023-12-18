using BootstrapBlazor.Components;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;
using System.Linq.Expressions;

namespace OrderTrackBlazor.Services
{
  public class SelectedItemService : ISelectedItemService
  {
    private readonly IContext context;

    public SelectedItemService(IContext context)
    {
      this.context = context;
    }
    public async Task<IEnumerable<SelectedItem>> GetEntitySelection<T>(Expression<Func<T, bool>>? display) where T : class, ISelectedItemEntity
    {
      var query = context.Query<T>(false);
      if (display != null)
      {
        query = query.Where(display);
      }
      var result = await query.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Id).Select(b => new SelectedItem { Value = b.Id.ToString(), Text = b.Name == null ? "" : b.Name }).ToArrayAsync();
      return (new SelectedItem[] { new SelectedItem("", "selected") }).Concat(result);
    }
  }
}
