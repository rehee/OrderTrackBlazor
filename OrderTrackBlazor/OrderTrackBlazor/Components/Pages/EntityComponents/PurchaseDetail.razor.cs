using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.DTOs;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class PurchaseDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    [Parameter]
    public List<OrderTrackProduction>? Productions { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();
    [Parameter]
    public PurchaseDTO? DTO { get; set; }
    public PurchaseDTO? Model { get; set; }
    public SelectedItem? SelectedShop { get; set; }
    public async Task SelectShop(SelectedItem item)
    {
      await Task.CompletedTask;
      if (Model == null)
      {
        return;
      }
      if (item == null)
      {
        Model.ShopId = null;
      }
      if (long.TryParse(item.Value, out var longId))
      {
        Model.ShopId = longId;
      }
    }
    [Inject]
    public IOrderService? orderService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();



      Model = DTO == null ? new PurchaseDTO
      {
        PurchaseDate = DateTime.UtcNow.Date,
        Productions = new List<OrderProductionDTO>()
      } : DTO;
      SelectedShop = Shops.FirstOrDefault(b => b.Value == Model?.ShopId?.ToString());

      StateHasChanged();
    }



    public async Task AddProduction(OrderProductionDTO? dto = null)
    {
      var fromTable = dto != null;
      await dialogService.ShowComponent<OrderProduction>(
        new Dictionary<string, object?>()
        {
          ["Productions"] = Productions,
          ["FromTable"] = fromTable,
          ["DTO"] = dto == null ? new OrderProductionDTO { ParentId = Model?.Id, Parent = Model } : dto
        },
       dto == null ? "new order production" : "Edit order production",
       true,
       async save => StateHasChanged()
       );
    }

    public override async Task<bool> SaveFunction()
    {
      if (Model.IsCreate)
      {
        var purcher = new OrderTrackPurchaseRecord()
        {
          PurchaseDate = Model.PurchaseDate?.Date,
          ShopId = Model.ShopId,
        };
        await Context.AddAsync<OrderTrackPurchaseRecord>(purcher, CancellationToken.None);
        foreach (var p in Model.Productions ?? new List<OrderProductionDTO>())
        {
          var production = new OrderTrackPurchaseItem
          {
            PurchaseRecordId = purcher.Id,
            PurchaseRecord = purcher,
            ProductionId = p.ProductionId,
            Quantity = p.Quantity ?? 0,
          };
          await Context.AddAsync<OrderTrackPurchaseItem>(production, CancellationToken.None);
        }
      }
      else
      {
        var purcher =
          Context.Query<OrderTrackPurchaseRecord>(false).FirstOrDefault(b => b.Id == Model.Id);
        if (purcher != null)
        {
          purcher.PurchaseDate = Model.PurchaseDate?.Date;
          purcher.ShopId = Model.ShopId;

          foreach (var p in Model.Productions ?? new List<OrderProductionDTO>())
          {
            if (p.IsCreate)
            {
              var production = new OrderTrackPurchaseItem
              {
                PurchaseRecordId = purcher.Id,
                PurchaseRecord = purcher,
                ProductionId = p.ProductionId,
                Quantity = p.Quantity ?? 0,
              };
              await Context.AddAsync<OrderTrackPurchaseItem>(production, CancellationToken.None);
            }
            else
            {
              var production =
                Context.Query<OrderTrackPurchaseItem>(false).FirstOrDefault(b => b.Id == p.Id);
              if (production == null)
              {
                continue;
              }
              production.Quantity = p.Quantity ?? 0;
              production.ProductionId = p.ProductionId;
            }

          }
        }

      }
      try
      {
        await Context.SaveChangesAsync(null);
      }
      catch
      {

      }


      StateHasChanged();
      return true;
    }
  }

}