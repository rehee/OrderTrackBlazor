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

    public List<(string?, int?, decimal)> Summary
    {
      get
      {
        var result = Dispatches.GroupBy(b => b.Status).OrderBy(b => b.Key).Select(b => (b.Key.ToString(), b.Sum(b => b.PackageNumber), b.Sum(b => b.TotalIncome))).ToList();
        return result;
      }
    }




    public async Task RefreshPage()
    {
      Dispatches = await dispatchService.GetDispatch(Id ?? 0)
        .OrderByDescending(b => b.DispatchDate).ThenByDescending(b => b.CreateDate).ToListAsync();
      StateHasChanged();
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await RefreshPage();
    }
    public async Task CreateDispatch(long? id = null)
    {
      await dialogService.ShowComponent<DispatchDetail>(
        new Dictionary<string, object?>
        {
          ["OrderId"] = Id,
          ["Id"] = id,
        },
        id == null ? "Create Dispatch" : "Edit Dispatch",
        true,
        async (b) => await RefreshPage()
        );
    }

    public async Task ReadDispatch(long? id = null)
    {
      await dialogService.ShowComponent<ViewDispatch>(
        new Dictionary<string, object?>()
        {
          ["DTO"] = Dispatches.FirstOrDefault(b => b.Id == id),
          ["ShowDetail"] = true,
        },
        "Dispatch"
        );
    }
  }

}