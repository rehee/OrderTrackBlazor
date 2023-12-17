using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class DispatchDetailComponent : CBase
  {
    [Parameter]
    public long? OrderId { get; set; }

    [Parameter]
    public long? Id { get; set; }

    [Inject]
    public IDispatchService? dispatchService { get; set; }
    public DispatchDetailDTO? Model { get; set; }
    public IEnumerable<DispatchDetailItemDTO> Source => Id == null ? Model.Items : Model.EditItems;
    public List<SelectedItem> StatusItem { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedStatusItem { get; set; }
    public Task OnItemChanged(SelectedItem item)
    {
      if (Model != null && Enum.TryParse<EnumDispatchStatus>(item.Value, out var result))
      {
        Model.Status = result;
      }
      return Task.CompletedTask;
    }
    public async Task recalculatePackageNumber()
    {
      await Task.CompletedTask;
      var map = new Dictionary<long?, int>();
      foreach (var package in Model.SourcePackages)
      {
        foreach (var item in package.Source)
        {
          if (!map.ContainsKey(item.ProductionId))
          {
            map.Add(item.ProductionId, 0);
          }
          map[item.ProductionId] = map[item.ProductionId] + ((package.Number ?? 0) * item.Number);
        }
      }
      foreach (var item in Source)
      {
        if (!map.ContainsKey(item.ProductionId))
        {
          continue;
        }
        item.NumberFromPackage = map[item.ProductionId];
      }
      StateHasChanged();
    }
    public Task PackageUpdate(int? value)
    {
      return Task.Run(async () =>
      {
        await recalculatePackageNumber();
      });
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      StatusItem.Add(new SelectedItem("", "selected"));

      foreach (int i in Enum.GetValues(typeof(EnumDispatchStatus)))
      {
        StatusItem.Add(new SelectedItem(i.ToString(), ((EnumDispatchStatus)i).ToString()));
      }
      if (Id == null)
      {
        Model = await dispatchService!.GetNewDispatchDetailDTO(OrderId ?? 0);
      }
      else
      {
        Model = await dispatchService!.FindDispatchDetailDTO(Id.Value);
        SelectedStatusItem = StatusItem.Where(b => b.Value == ((int)Model.Status).ToString()).FirstOrDefault();
      }
      await recalculatePackageNumber();
      StateHasChanged();
    }
    public override async Task<bool> SaveFunction()
    {
      await recalculatePackageNumber();
      if (Id == null)
      {
        return await dispatchService.CreateDispatch(Model);
      }
      else
      {
        return await dispatchService.UpdateDispatch(Model);
      }

    }
  }
}