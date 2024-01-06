using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class StockPurchaseProductionComponent : CBase
  {
    [Parameter]
    public StockPurchaseDTO DTO { get; set; }
    [Parameter]
    public IEnumerable<SelectedItem> ProductionSelections { get; set; } = Enumerable.Empty<SelectedItem>();

    public int? Quanty { get; set; }
    public SelectedItem? SelectedItem { get; set; }

    public string[] Ids { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Ids = DTO.Ids?.ToArray() ?? Array.Empty<string>();
    }
    public override async Task<bool> SaveFunction()
    {
      await base.SaveFunction();
      if ((Quanty ?? 0) == 0 || SelectedItem == null)
      {
        return false;
      }
      int.TryParse(SelectedItem.Value, out var number);
      if (number <= 0)
      {
        return false;
      }
      DTO.RequestInput.Add(new StockRequireSummaryDTO
      {
        ProductionId = number,
        ProductionName = SelectedItem.Text,
        Number = Quanty,
      });

      return true;
    }
    public IEnumerable<SelectedItem> DisplayProductionSelections => ProductionSelections.Where(b => !Ids.Contains(b.Value));
  }
}