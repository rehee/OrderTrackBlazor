using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ViewDispatchComponent : CBase
  {
    [Parameter]
    public DispatchDetailDTO? DTO { get; set; }
    [Parameter]
    public bool ShowDetail { get; set; }
  }
}