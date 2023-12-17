using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PackageDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    [Parameter]
    public long OrderId { get; set; }
    [Parameter]
    public bool? ReadOnly { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedItem { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      if (Model != null && long.TryParse(item.Value, out var result))
      {
        Model.SizeId = result;
      }
      return Task.CompletedTask;
    }
    [Inject]
    public IPackageService Service { get; set; }

    public PackageDetailDTO? Model { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }
    public async Task Refresh()
    {
      Model = Id == null ? await Service.GetNewPackageDTO(OrderId) : await Service.GetPackageDTO(Id.Value);
      if (Id != null)
      {
        SelectedItem = PackageSizes.Where(b => b.Value == Model.SizeId?.ToString()).FirstOrDefault();
      }
    }

    public override async Task<bool> SaveFunction()
    {
      if (Id == null)
      {
        await Service.CreatePackageAsync(Model);
      }
      else
      {
        await Service.UpdatePackageAsync(Model);
      }
      return true;
    }
  }


}