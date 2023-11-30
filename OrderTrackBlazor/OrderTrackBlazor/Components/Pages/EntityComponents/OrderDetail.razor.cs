using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Entities;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class OrderDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }

    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    [Inject]
    public IOrderService? orderService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();
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
      StateHasChanged();
    }
    public OrderDTO? Model { get; set; }
    public Func<Task<bool>>? onSaveAsync = null;

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
            Quantity = p.Quantity,
            OrderPrice = p.OrderPrice,
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
              Quantity = p.Quantity,
              OrderPrice = p.OrderPrice,
            };
            await Context.AddAsync<OrderTrackOrderItem>(p2, CancellationToken.None);
          }
          else
          {
            var p2 = Context.Query<OrderTrackOrderItem>(false).FirstOrDefault(b => b.Id == p.Id);
            if (p2 != null)
            {
              p2.ProductionId = p.ProductionId;
              p2.Quantity = p.Quantity;
              p2.OrderPrice = p.OrderPrice;
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