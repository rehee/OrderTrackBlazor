
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class CategoryPageComponent : CBase
  {
    public IEnumerable<OrderTrackCategory> Categories { get; set; } = Enumerable.Empty<OrderTrackCategory>();

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }

    public async Task Refresh()
    {
      Categories = await Context.Query<OrderTrackCategory>(true).OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name).ToArrayAsync();
      StateHasChanged();
    }

    public async Task MaintainDetail(long? id = null)
    {
      await dialogService.ShowComponent<CategoryDetail>(new Dictionary<string, object?>
      {
        ["Id"] = id
      },
      id == null ? "Create Category" : "Edit Category",
      true,
      async save => await Refresh()
      );
    }
  }
}