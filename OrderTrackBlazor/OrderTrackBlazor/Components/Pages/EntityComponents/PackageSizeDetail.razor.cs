using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PackageSizeDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    public OrderTrackPackageSize? PackageSize { get; set; }

    public bool DeleteOnSave { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (Id == null)
      {
        PackageSize = new OrderTrackPackageSize() { DisplayOrder = 1 };
      }
      else
      {
        PackageSize = await Context.Query<OrderTrackPackageSize>(true).Where(x => x.Id == Id).FirstOrDefaultAsync();
      }
    }

    public override async Task<bool> SaveFunction()
    {
      if (Id == null)
      {
        if (DeleteOnSave)
        {
          return true;
        }
        await Context.AddAsync<OrderTrackPackageSize>(new OrderTrackPackageSize
        {
          Name = PackageSize.Name,
          DisplayOrder = PackageSize.DisplayOrder,
          WeightGram = PackageSize.WeightGram,
        }, CancellationToken.None);
        await Context.SaveChangesAsync(null);
      }
      else
      {

        var record = await Context.Query<OrderTrackPackageSize>(false).Where(x => x.Id == Id).FirstOrDefaultAsync();
        if (DeleteOnSave)
        {
          Context.Delete<OrderTrackPackageSize>(record);
          await Context.SaveChangesAsync(null);
          return true;
        }
        if (PackageSize != null && record != null)
        {
          record.Name = PackageSize?.Name;
          record.DisplayOrder = PackageSize?.DisplayOrder ?? 1;
          record.WeightGram = PackageSize?.WeightGram;
        }
        await Context.SaveChangesAsync(null);
      }

      return true;
    }
  }
}
