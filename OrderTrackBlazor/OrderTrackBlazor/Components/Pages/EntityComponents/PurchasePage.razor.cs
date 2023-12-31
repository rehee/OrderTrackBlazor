﻿using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Consts;
using System.Text;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class PurchasePageComponent : CBase
  {
    [Parameter]
    public long? ProductionId { get; set; }
    public List<OrderTrackProduction> Productions { get; set; } = new List<OrderTrackProduction>();
    public IEnumerable<SelectedItem> Shops { get; set; } = new List<SelectedItem>();

    public List<PurchaseDTO> Purchase { get; set; } = new List<PurchaseDTO>();
    public List<PurchaseListDTO> PurchaseItemList { get; set; } = new List<PurchaseListDTO>();
    [Inject]
    public IPurchaseService? purchaseService { get; set; }
    [Inject]
    public IShopService? shopService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Productions = await Context.Query<OrderTrackProduction>(true).ToListAsync();

      Shops = await shopService.GetShopSelected();
      await Refresh();
    }

    public async Task Refresh()
    {
      Purchase = await
        (ProductionId.HasValue ? purchaseService.Query().Where(b => b.Productions.Any(b => b.ProductionId == ProductionId)) : purchaseService.Query())
        .OrderByDescending(b => b.PurchaseDate).ThenByDescending(b => b.CreateDate).ToListAsync();
      foreach (var p in Purchase)
      {
        foreach (var item in p.Productions)
        {
          item.Parent = p;
        }
      }
      PurchaseItemList.Clear();
      foreach (var p in Purchase)
      {
        foreach (var item in (p.Productions ?? new List<OrderProductionDTO>()).OrderByDescending(b => b.CreateDate))
        {
          if (ProductionId.HasValue && item.ProductionId != ProductionId)
          {
            continue;
          }
          PurchaseItemList.Add(
            new PurchaseListDTO
            {
              Id = p.Id,
              ItemId = item.Id,
              ProductionId = item.ProductionId,
              PurchaseDate = p.PurchaseDate,
              CreateDate = item.CreateDate,
              ShopId = p.ShopId,
              Quantity = item.Quantity ?? 0,
            });
        }
      }
      StateHasChanged();
    }

    public async Task NormalShowDialog(PurchaseDTO? dto = null)
    {
      await dialogService.ShowComponent<PurchaseDetail>(
        new Dictionary<string, object?>()
        {
          ["Productions"] = Productions,
          ["Shops"] = Shops,
          ["DTO"] = dto
        },
       DefaultValues.PTitle,
       true,
       async save => await Refresh()
       );
    }

  }
}
