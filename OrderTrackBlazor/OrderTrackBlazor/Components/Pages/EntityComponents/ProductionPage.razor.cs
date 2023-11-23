using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.DTOs;
using ReheeCmf.Reflects.ReflectPools;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ProductionPageComponent : CBase
  {
    [Parameter]
    public string? EntityName { get; set; }

    public Type? EntityType { get; set; }

    public List<OrderTrackProduction>? Productions { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshTable();
    }
    public async Task RefreshTable()
    {

      Productions = await Context!.Query<OrderTrackProduction>(true).ToListAsync();
      StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      
    }

    public async Task CreateProduction(long? id = null)
    {
      var model = id == null ?
        new ProductionDTO() :
        await Context!.Query<OrderTrackProduction>(true).Where(b => b.Id == id).Select(b => new ProductionDTO() { Id = b.Id, ProductionName = b.Name }).FirstOrDefaultAsync();
      var items = Utility.GenerateEditorItems<ProductionDTO>();
      if (id != null)
      {

      }
      var option = new EditDialogOption<ProductionDTO>()
      {
        Title = "edit dialog",
        Model = model,
        Items = items,
        ItemsPerRow = 1,
        RowType = RowType.Normal,
        OnCloseAsync = () =>
        {

          return Task.CompletedTask;
        },
        OnEditAsync = async (context) =>
        {
          if (context.Model is ProductionDTO mdl)
          {
            if (mdl == null)
            {
              return true;
            }
            if (mdl.Id == null)
            {
              if (String.IsNullOrEmpty(mdl.ProductionName))
              {
                return false;
              }
              await Context!.AddAsync<OrderTrackProduction>(new OrderTrackProduction
              {
                Name = mdl.ProductionName
              }, CancellationToken.None);
              await Context.SaveChangesAsync(null);
              await RefreshTable();
              return true;
            }
            else
            {
              var p = await Context!.Query<OrderTrackProduction>(false).Where(b => b.Id == mdl.Id).FirstOrDefaultAsync();
              if (p == null)
              {
                return true;
              }
              p.Name = mdl.ProductionName;
              await Context!.SaveChangesAsync(null);
              await RefreshTable();
              return true;
            }
          }
          return true;
        }
      };
      await dialogService.ShowEditDialog(option);
    }
    public OrderTrackProduction? Model { get; set; }

    
  }
}