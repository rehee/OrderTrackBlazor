using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using ReheeCmf.Helpers;

namespace OrderTrackBlazor.Components.Pages
{
  public class AvaliableStockComponent : CBase
  {
    private DotNetObjectReference<AvaliableStockComponent> dotnetObjRef;
    [Parameter]
    public string? slide { get; set; }
    [Inject]
    public IStockService? stockService { get; set; }
    public IEnumerable<StockSummaryDTO> SummaryDTOs { get; set; } = Enumerable.Empty<StockSummaryDTO>();
    public static DateTime? AvaliableUntil { get; set; }
    public WebSpeech? WebSpeech { get; set; }
    public Carousel? crousel { get; set; }

    public StockSummaryDTO[] SummaryDTOsArray => SummaryDTOs.OrderByDescending(b => b.CategoryDisplayOrder)
    .ThenBy(b => b.CategoryDisplayOrder)
    .ThenBy(b => b.CategoryName).ToArray();
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      dotnetObjRef = DotNetObjectReference.Create(this);
      if (AvaliableUntil == null || AvaliableUntil.Value < DateTime.UtcNow)
      {
        return;
      }
      SummaryDTOs = await stockService.QuerySummary().Where(b => b.CurrentStock > 0)
        .OrderBy(b => b.Name).ToArrayAsync();

    }

    [Inject]
    public IJSRuntime js { get; set; }
    public bool FirstRead = false;
    public bool Delaied = false;
    public bool loaded = false;
    int count = 0;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);
      if (slide != null)
      {
        if (loaded != true)
        {
          loaded = SummaryDTOs?.Any() == true;
        }
        if (loaded)
        {
          await MyCallbackFunction("0");
          await js.InvokeVoidAsync("bcOnInvoke.invoke", "C1", dotnetObjRef);
        }
      }
    }
    public int _circleValue { get; set; }
    public async Task DelayTask()
    {
      do
      {
        await Task.Delay(100);
        _circleValue = _circleValue + 1;
        StateHasChanged();
      } while (_circleValue < 100);

    }
    private bool reading = false;
    public async Task Speak(string? text = null)
    {
      if (reading == false)
      {
        reading = true;
      }
      else
      {
        await WebSpeech.SpeechStop();
      }
      if (long.TryParse(text ?? "", out var longId))
      {
        var item = SummaryDTOsArray[longId];
        await WebSpeech.SpeechSynthesis($"{item.CategoryName}, {item.Name}, ÊýÁ¿ {item.CurrentStock}", "zh-CN");
      }

      reading = false;
    }

    public override async ValueTask DisposeAsync()
    {
      await base.DisposeAsync();
      dotnetObjRef = null;
    }

    [JSInvokable]
    public async Task MyCallbackFunction(string message)
    {
      await Speak(message);
    }
  }
}