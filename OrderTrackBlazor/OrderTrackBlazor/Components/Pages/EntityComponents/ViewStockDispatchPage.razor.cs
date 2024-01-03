using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ViewStockDispatchPageComponent : CBase
  {
    [Parameter]
    public long Id { get; set; }
    [Inject]
    public IStockDispatchService? service { get; set; }
    public StockDispatchDTO? DTO { get; set; }
    public List<SelectedItem> StatusItem { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedStatusItem { get; set; }
    [Inject]
    public IPackageService PackageService { get; set; }
    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      DTO = await service.FindDTO(Id);
    }
  }
}