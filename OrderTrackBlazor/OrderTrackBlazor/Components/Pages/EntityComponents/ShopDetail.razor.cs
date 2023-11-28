using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ShopDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    public OrderTrackShop? Shop { get; set; }

    public bool DeleteOnSave { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (Id == null)
      {
        Shop = new OrderTrackShop() { DisplayOrder = 1 };
      }
      else
      {
        Shop = await Context.Query<OrderTrackShop>(true).Where(x => x.Id == Id).FirstOrDefaultAsync();
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
        await Context.AddAsync<OrderTrackShop>(new OrderTrackShop
        {
          Note = Shop?.Note,
          PostCode = Shop?.PostCode,
          ShopName = Shop?.ShopName,
          DisplayOrder = Shop?.DisplayOrder ?? 1
        }, CancellationToken.None);
        await Context.SaveChangesAsync(null);
      }
      else
      {

        var record = await Context.Query<OrderTrackShop>(false).Where(x => x.Id == Id).FirstOrDefaultAsync();
        if (DeleteOnSave)
        {
          Context.Delete<OrderTrackShop>(record);
          await Context.SaveChangesAsync(null);
          return true;
        }
        if (Shop != null && record != null)
        {
          record.Note = Shop?.Note;
          record.PostCode = Shop?.PostCode;
          record.ShopName = Shop?.ShopName;
          record.DisplayOrder = Shop?.DisplayOrder ?? 1;
        }
        await Context.SaveChangesAsync(null);
      }

      return true;
    }
  }
}
