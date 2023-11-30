using BootstrapBlazor.Components;
using Google.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
      StateHasChanged();
    }
    public override async Task<bool> SaveFunction()
    {
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