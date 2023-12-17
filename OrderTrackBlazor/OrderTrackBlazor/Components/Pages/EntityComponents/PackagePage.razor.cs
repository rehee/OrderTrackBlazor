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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<PackageDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
            ["OrderId"] = Id,
            ["PackageSizes"] = PackageSizes,
            ["OnSave"] = onsave,
            ["ReadOnly"] = readOnly,
          });
      var dotion = new DialogOption()
      {
        Title = id == null ? "new package" : "edit package",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = readOnly != true,
        OnSaveAsync = async () =>
        {
          if (onsave.OnSaveFunc != null)
          {
            await onsave.OnSaveFunc();
          }
          await Refresh();
          return true;
        }
      };
      await dialogService!.Show(dotion);
    }
  }


}