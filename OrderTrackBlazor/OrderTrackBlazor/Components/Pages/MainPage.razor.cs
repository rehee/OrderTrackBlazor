using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages
{
  public partial class MainPage
  {
    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public RenderFragment? Main { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public bool ShowFooter { get; set; }
  }
}