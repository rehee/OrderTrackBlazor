using BootstrapBlazor.Components;

namespace OrderTrackBlazor.Services
{
  public interface IShopService
  {
    Task<IEnumerable<SelectedItem>> GetShopSelected();
  }
}
