using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ProductionDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    public ProductionDTO? Production { get; set; }

    [Inject]
    public IProductionService ProductionService { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }

    public async Task Refresh()
    {
      Production = await ProductionService.GetProduction(Id);
      StateHasChanged();
    }

    public override async Task<bool> SaveFunction()
    {
      var result = await ProductionService.SaveChange(Production);
      if (OnSave != null)
      {
        OnSave.ResultValue = Production.NewId;
      }
      return result;

    }
  }
}