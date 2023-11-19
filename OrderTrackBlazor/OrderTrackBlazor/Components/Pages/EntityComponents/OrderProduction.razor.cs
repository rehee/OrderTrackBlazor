using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class OrderProductionComponent : CBase
  {
    [Parameter]
    public OrderProductionDTO? DTO { get; set; }
    [Parameter]
    public OnSaveDTO? OnSave { get; set; }
    [Parameter]
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();

    public List<SelectedItem> Items = new List<SelectedItem>();
    public SelectedItem? IItem { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Items = (new List<SelectedItem>() { new SelectedItem("", "select") }).Concat(Productions.Select(b => new SelectedItem(b.Id.ToString() ?? "", b.Name ?? ""))).ToList();
      IItem = Items.FirstOrDefault(b => b.Value == DTO?.Id.ToString());
      if (OnSave != null)
      {
        OnSave.OnSaveFunc = async () =>
        {
          if (DTO == null || DTO.Parent == null)
          {
            return true;
          }
          if (long.TryParse(IItem?.Value, out var lId))
          {
            if (lId >= 0)
            {
              DTO.ProductionId = lId;
            }
          }
          if (DTO.ProductionId <= 0)
          {
            return true;
          }
          if (DTO.IsCreate && DTO.Quantity <= 0)
          {
            return true;
          }
          DTO.ParentId = DTO.Parent.Id;
          DTO.Parent.Productions?.Add(DTO);
          return true;
        };
      }
      StateHasChanged();

    }



  }

}