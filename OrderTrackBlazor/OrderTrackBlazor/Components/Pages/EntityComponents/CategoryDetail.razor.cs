using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class CategoryDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }

    public OrderTrackCategory? Category { get; set; } = null;

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (Id != null)
      {
        Category = await Context.Query<OrderTrackCategory>(false).Where(b => b.Id == Id).FirstOrDefaultAsync();
      }
      else
      {
        Category = new OrderTrackCategory();
      }
    }

    public override async Task<bool> SaveFunction()
    {
      await base.SaveFunction();
      if (Category?.Id <= 0)
      {
        await Context.AddAsync(Category);
      }
      await Context.SaveChangesAsync();
      return true;
    }
  }
}