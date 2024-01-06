using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Entities;
using OrderTrackBlazor.Helpers;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class OrderDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }

    public IEnumerable<ProductionDTO> Productions { get; set; } = Enumerable.Empty<ProductionDTO>();

    public IEnumerable<SelectedItem> ShopSelect { get; set; } = Enumerable.Empty<SelectedItem>();
    [Inject]
    public IOrderService? orderService { get; set; }
    [Inject]
    public ISelectedItemService? selectedItemService { get; set; }
    [Inject]
    public IProductionService productionService { get; set; }

    public async Task RefreshProduction()
    {
      Productions = await productionService.GetAllProductions();
    }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshProduction();
      if (Id == null || Id <= 0)
      {
        Model = new OrderDTO
        {
          Id = -1,
          Productions = new List<OrderProductionDTO>()
        };
      }
      else
      {
        Model = await orderService.FindAsync(Id);
      }
      ShopSelect = (await selectedItemService.GetEntitySelection<OrderTrackShop>()).ToList();
      StateHasChanged();
    }
    public OrderDTO? Model { get; set; }
    public Func<Task<bool>>? onSaveAsync = null;

    public async Task CreateProduction(Action<IEnumerable<ProductionDTO>, long> parantAction)
    {
      await dialogService.ShowComponent<ProductionDetail>(
        null,
        "create Product",
        true,
        async save =>
        {
          await RefreshProduction();
          StateHasChanged();
          long id = 0;
          if (save.ResultValue != null)
          {
            if (save.ResultValue is long longId)
            {
              id = longId;
            }
          }
          parantAction(Productions, id);
        }
        );
    }

    public async Task AddProduction(OrderProductionDTO? dto = null)
    {
      Func<Action<IEnumerable<ProductionDTO>, long>, Task> createProductionFunc = t => CreateProduction(t);
      var fromTable = dto != null;
      await dialogService!.ShowComponent<OrderProduction>(
        new Dictionary<string, object?>
        {
          ["Productions"] = Productions,
          ["FromTable"] = fromTable,
          ["ShopSelect"] = ShopSelect,
          ["DTO"] = dto == null ? new OrderProductionDTO { ParentId = Model?.Id, Parent = Model } : dto,
          ["CreateProduction"] = createProductionFunc
        },
        dto == null ? "new order production" : "new order production",
        true,
        async save =>
        {
          await Task.CompletedTask;
          StateHasChanged();
        }
        );
    }

    public override async Task<bool> SaveFunction()
    {
      if (Model.IsCreate)
      {
        var order = new OrderTrackOrder
        {
          Note = Model.Note,
          ShortNote = Model.ShortNote,
          OrderDate = Model.OrderDate,
        };
        await Context.AddAsync<OrderTrackOrder>(order, CancellationToken.None);
        foreach (var p in Model.Productions ?? new List<OrderProductionDTO>())
        {
          var p2 = new OrderTrackOrderItem()
          {
            OrderTrackOrderId = order.Id,
            Order = order,
            ProductionId = p.ProductionId,
            Quantity = p.Quantity ?? 0,
            OrderPrice = p.OrderPrice,
            RecommendShopId = p.RecommandShopId,
            RecommendShopId2 = p.RecommandShopId2,
            Note = p.Note,
          };
          await Context.AddAsync<OrderTrackOrderItem>(p2, CancellationToken.None);
        }
      }
      else
      {
        var order = Context.Query<OrderTrackOrder>(false).FirstOrDefault(b => b.Id == Model.Id);
        if (order != null)
        {
          order.Note = Model.Note;
          order.ShortNote = Model.ShortNote;
          order.OrderDate = Model.OrderDate;

        }
        foreach (var p in Model.Productions ?? new List<OrderProductionDTO>())
        {
          if (p.IsCreate)
          {
            var p2 = new OrderTrackOrderItem()
            {
              OrderTrackOrderId = order.Id,
              Order = order,
              ProductionId = p.ProductionId,
              Quantity = p.Quantity ?? 0,
              OrderPrice = p.OrderPrice,
              RecommendShopId = p.RecommandShopId,
              RecommendShopId2 = p.RecommandShopId2,
              Note = p.Note,
            };
            await Context.AddAsync<OrderTrackOrderItem>(p2, CancellationToken.None);
          }
          else
          {
            var p2 = Context.Query<OrderTrackOrderItem>(false).FirstOrDefault(b => b.Id == p.Id);
            if (p2 != null)
            {
              p2.ProductionId = p.ProductionId;
              p2.Quantity = p.Quantity ?? 0;
              p2.OrderPrice = p.OrderPrice;
              p2.RecommendShopId = p.RecommandShopId;
              p2.RecommendShopId2 = p.RecommandShopId2;
              p2.Note = p.Note;
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