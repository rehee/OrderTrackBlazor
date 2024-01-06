using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PackagePageComponent : CBase
  {
    [Parameter]
    public long Id { get; set; }

    [Inject]
    public IPackageService Service { get; set; }

    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    public List<PackageDetailDTO> Lists { get; set; } = new List<PackageDetailDTO>();
    protected override async Task OnInitializedAsync()
    {
      PackageSizes = await Service.GetPackageSize();
      await Refresh();
    }
    public async Task Refresh()
    {
      Lists = await Service.GetAllPackages(Id);
      StateHasChanged();
    }

    public async Task CreatePackage(long? id = null, bool? readOnly = null)
    {
      await dialogService.ShowComponent<PackagePage>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,
          ["OrderId"] = Id,
          ["PackageSizes"] = PackageSizes,
          ["ReadOnly"] = readOnly,
        },
       id == null ? "new package" : "edit package",
       true,
       async save => await Refresh()
       );
    }
  }


}