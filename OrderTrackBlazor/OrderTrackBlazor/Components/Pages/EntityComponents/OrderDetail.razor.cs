using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class OrderDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    [Parameter]
    public OnSaveDTO? OnSave { get; set; }
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();
      if (Id == null || Id <= 0)
      {
        Model = new OrderDTO
        {
          Productions = new List<OrderProductionDTO>()
        };
      }
      if (OnSave != null)
      {
        OnSave.OnSaveFunc = async () =>
        {

          return true;
        };
      }

      StateHasChanged();
    }
    public OrderDTO? Model { get; set; }
    public Func<Task<bool>>? onSaveAsync = null;

    public async Task AddProduction(OrderProductionDTO? dto = null)
    {

      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<OrderProduction>(
          new Dictionary<string, object?>()
          {
            ["OnSave"] = onsave,
            ["Productions"] = Productions,
            ["DTO"] = dto == null ? new OrderProductionDTO { ParentId = Model?.Id, Parent = Model } : dto
          });

      var dotion = new DialogOption()
      {
        Title = dto == null ? "new order production" : "new order production",
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          if (onsave.OnSaveFunc != null)
          {
            await onsave.OnSaveFunc();
          }
          StateHasChanged();
          return true;
        }
      };
      await dialogService!.Show(dotion);
    }


  }

}