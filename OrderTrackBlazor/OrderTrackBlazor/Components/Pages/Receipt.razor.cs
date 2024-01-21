using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ReheeCmf.Commons;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Helpers;
using System.Net;

namespace OrderTrackBlazor.Components.Pages
{
  public class ReceiptComponent : CBase
  {
    private DotNetObjectReference<AvaliableStockComponent> dotnetObjRef;
    [Parameter]
    public string ids { get; set; }
    [Inject]
    public IPurchaseService? service { get; set; }
    public IEnumerable<StockSummaryDTO> SummaryDTOs { get; set; } = Enumerable.Empty<StockSummaryDTO>();
    public static DateTime? AvaliableUntil { get; set; }
    public WebSpeech? WebSpeech { get; set; }
    public Carousel? crousel { get; set; }

    public StockSummaryDTO[] SummaryDTOsArray => SummaryDTOs.OrderByDescending(b => b.CategoryDisplayOrder)
    .ThenBy(b => b.CategoryDisplayOrder)
    .ThenBy(b => b.CategoryName).ToArray();

    [CascadingParameter]
    private Task<AuthenticationState>? authenticationState { get; set; }

    public IEnumerable<PurchaseDTO> DTOs { get; set; } = Enumerable.Empty<PurchaseDTO>();

    protected override async Task OnInitializedAsync()
    {
      var longId = ids.Split(",").Select(b =>
      {
        long.TryParse(b, out var id);
        return (long?)id;
      }).Where(b => b > 0).ToArray();
      DTOs = await service.Query().Where(b => longId.Contains(b.Id)).ToArrayAsync();
    }
  }
}