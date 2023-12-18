using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public partial class OrderProductionComponent : CBase
  {
    [Parameter]
    public bool? FromTable { get; set; }
    [Parameter]
    public OrderProductionDTO? DTO { get; set; }
    [Parameter]
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();

    [Parameter]
    public IEnumerable<SelectedItem>? ShopSelect { get; set; } = Enumerable.Empty<SelectedItem>();
    public SelectedItem? SelectedShop { get; set; }

    public async Task OrderItemChange(SelectedItem item)
    {
      await Task.CompletedTask;
      if (item == null || DTO == null)
      {
        return;
      }
      long.TryParse(item.Value, out var id);
      var selectProduction = Productions.Where(b => b.Id == id).FirstOrDefault();
      if (selectProduction == null)
      {
        return;
      }
      DTO.OrderPrice = selectProduction.OriginalPrice;
      DTO.ProductionId = id;
    }
    public async Task ShopItemChange(SelectedItem item)
    {
      await Task.CompletedTask;
      if (item == null || DTO == null)
      {
        return;
      }
      if (long.TryParse(item.Value, out var id))
      {
        DTO.RecommandShopId = id;
        DTO.RecommandShopName = item.Text;
      }
      else
      {
        DTO.RecommandShopId = null;
        DTO.RecommandShopName = null;
      }

    }
    public List<SelectedItem> Items = new List<SelectedItem>();
    public SelectedItem? IItem { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Items = (new List<SelectedItem>() { new SelectedItem("", "select") }).Concat(Productions.Select(b => new SelectedItem(b.Id.ToString() ?? "", b.Name ?? ""))).ToList();
      IItem = Items.FirstOrDefault(b => b.Value == DTO?.ProductionId.ToString());
      SelectedShop = ShopSelect.FirstOrDefault(b => b.Value == DTO?.RecommandShopId.ToString());
      StateHasChanged();

    }


    public override async Task<bool> SaveFunction()
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
      else
      {
        return false;
      }
      if (DTO.ProductionId <= 0)
      {
        return true;
      }
      if (DTO.Quantity <= 0)
      {
        DTO.Quantity = 0;
      }
      if (DTO.IsCreate && FromTable == true && DTO.Quantity <= 0)
      {
        DTO.Parent.Productions?.Remove(DTO);
        return true;
      }
      if (DTO.IsCreate && DTO.Quantity <= 0)
      {
        return true;
      }

      if (DTO.IsCreate)
      {
        DTO.ParentId = DTO.Parent.Id;
        var exist = DTO.Parent.Productions?.Where(b => b.ProductionId == DTO.ProductionId).FirstOrDefault();
        if (exist == null)
        {
          DTO.Parent.Productions?.Add(DTO);
        }
        else
        {
          if (FromTable != true)
          {
            exist.Quantity = exist.Quantity + DTO.Quantity;
          }
        }
      }
      return true;
    }
  }

}