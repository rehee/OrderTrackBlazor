using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

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
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<PackageSizeDetail>(
          new Dictionary<string, object?>()
          {
            ["Id"] = id,
            ["OnSave"] = onsave,
          });
      var dotion = new DialogOption()
      {
        Title = $"{(id == null ? "Create" : "Edit")}  Package Size",
        Size = Size.Medium,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onsave.OnSaveFunc != null)
          {
            result = await onsave.OnSaveFunc();
          }
          await RefreshTable();
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }
  }
}