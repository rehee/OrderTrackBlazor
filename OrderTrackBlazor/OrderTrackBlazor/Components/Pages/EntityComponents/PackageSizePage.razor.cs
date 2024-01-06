using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PackageSizePageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }

    public Type? EntityType { get; set; }

    public List<OrderTrackPackageSize>? Packages { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    public async Task RefreshTable()
    {

      Packages = await Context!.Query<OrderTrackPackageSize>(true).OrderBy(b => b.DisplayOrder).ThenBy(b => b.Id).ToListAsync();
      StateHasChanged();
    }
    public async Task NormalShowDialog(long? id = null)
    {
      await dialogService.ShowComponent<PackageSizeDetail>(
        new Dictionary<string, object?>()
        {
          ["Id"] = id,

        },
       id == null ? "new Package Size" : "edit Package Size",
       true,
       async save => await RefreshTable()
       );
    }
  }
}