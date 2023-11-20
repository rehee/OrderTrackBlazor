using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.DTOs;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class PurchaseDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    [Parameter]
    public List<OrderTrackProduction>? Productions { get; set; }
    [Parameter]
    public List<OrderTrackShop>? Shops { get; set; }
    [Parameter]
    public PurchaseDTO? DTO { get; set; }
    public PurchaseDTO? Model { get; set; }

    public List<SelectedItem> ShopList { get; set; } = new List<SelectedItem>();
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
      ShopList = (new List<SelectedItem>() { new SelectedItem("", "select") })
        .Concat(
        Shops.Select(b => new SelectedItem(b.Id.ToString(), b.ShopType.ToString()))
        ).ToList();


      Model = DTO == null ? new PurchaseDTO
      {
        Productions = new List<OrderProductionDTO>()
      } : DTO;
      SelectedShop = ShopList.FirstOrDefault(b => b.Value == Model?.ShopId?.ToString());

      StateHasChanged();
    }



    public async Task AddProduction(OrderProductionDTO? dto = null)
    {
      var fromTable = dto != null;
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderProduction>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["Productions"] = Productions,
            ["FromTable"] = fromTable,
            ["DTO"] = dto == null ? new OrderProductionDTO { ParentId = Model?.Id, Parent = Model } : dto
          }); ;

      var dotion = new DialogOption()
      {
        Title = dto == null ? "new order production" : "new order production",
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onsave.OnSaveFunc != null)
          {
            result = await onsave.OnSaveFunc();
          }
          if (result)
          {
            StateHasChanged();
          }
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }

    public override async Task<bool> SaveFunction()
    {
      if (Model.IsCreate)
      {
        var purcher = new OrderTrackPurchaseRecord()
        {
          PurchaseDate = Model.PurchaseDate,
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
            Quantity = p.Quantity,
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
          purcher.PurchaseDate = Model.PurchaseDate;
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
                Quantity = p.Quantity,
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
              production.Quantity = p.Quantity;
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