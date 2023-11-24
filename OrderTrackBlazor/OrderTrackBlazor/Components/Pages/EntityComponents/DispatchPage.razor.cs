using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class DispatchPageComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    public List<DispatchListDTO> DispatchList { get; set; } = new List<DispatchListDTO>();
    [Inject]
    public IDispatchService? dispatchService { get; set; }

    public List<DispatchDetailDTO> Dispatches { get; set; } = new List<DispatchDetailDTO>();
    public async Task RefreshPage()
    {
      Dispatches = await dispatchService.GetDispatch(Id ?? 0)
        .OrderByDescending(b=>b.DispatchDate).ThenByDescending(b=>b.CreateDate).ToListAsync();
      StateHasChanged();
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshPage();
    }
    public async Task CreateDispatch(long? id = null)
    {
      var onSave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<DispatchDetail>(
          new Dictionary<string, object?>()
          {
            ["OrderId"] = Id,
            ["Id"] = id,
            ["OnSave"] = onSave
          });
      var dotion = new DialogOption()
      {
        Title = "Create Dispatch",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onSave.OnSaveFunc != null)
          {
            result = await onSave.OnSaveFunc();
          }
          await RefreshPage();
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }
  }

}