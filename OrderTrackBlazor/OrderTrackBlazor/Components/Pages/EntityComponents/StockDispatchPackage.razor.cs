using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class StockDispatchPackageComponent : CBase
  {
    [Parameter]
    public StockDispatchDTO? ParentDTO { get; set; }
    [Parameter]
    public StockDispatchPackageDTO? DTO { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedItem { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      if (DTO != null && long.TryParse(item.Value, out var result))
      {
        DTO.PackageSizeId = result;
      }
      return Task.CompletedTask;
    }
    [Inject]
    public IStockDispatchService? Service { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (DTO != null)
      {
        SelectedItem = PackageSizes.Where(b => b.Value == DTO.PackageSizeId?.ToString()).FirstOrDefault();
      }
    }
    public override async Task<bool> SaveFunction()
    {
      await Task.CompletedTask;
      if (DTO == null)
      {
        return true;
      }
      if (DTO.Id <= 0)
      {
        return await Service.Create(DTO, ParentDTO);

      }
      else
      {
        return await Service.Update(DTO);
      }

      return true;
    }
  }
}