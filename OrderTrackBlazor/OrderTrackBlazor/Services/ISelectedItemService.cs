using BootstrapBlazor.Components;
using System.Linq.Expressions;

namespace OrderTrackBlazor.Services
{
  public interface ISelectedItemService
  {
    Task<IEnumerable<SelectedItem>> GetEntitySelection<T>(Expression<Func<T, bool>>? display = null) where T : class, ISelectedItemEntity;
  }

  public interface ISelectedItemEntity
  {
    long Id { get; set; }
    string? Name { get; set; }
    int DisplayOrder { get; set; }
  }
}
