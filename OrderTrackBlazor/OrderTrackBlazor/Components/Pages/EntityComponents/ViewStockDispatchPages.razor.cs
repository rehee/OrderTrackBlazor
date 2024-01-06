using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ViewStockDispatchPagesComponent : CBase
  {
    [Parameter]
    public IEnumerable<long>? Ids { get; set; }
    [Parameter]
    public string? idsfromurl { get; set; }
    [Inject]
    public IStockDispatchService? service { get; set; }
    public StockDispatchDTO[] DTOs { get; set; } = Array.Empty<StockDispatchDTO>();
    public List<SelectedItem> StatusItem { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedStatusItem { get; set; }
    [Inject]
    public IPackageService PackageService { get; set; }
    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      var idParameter = Ids?.Any() == true ? Ids :
        idsfromurl.Split(",").Select(b =>
        {
          long.TryParse(b, out var num);
          return num;
        }).DistinctBy(b => b).Where(b => b > 0);
      DTOs = await service.FindDTO(idParameter);
    }

  }
}