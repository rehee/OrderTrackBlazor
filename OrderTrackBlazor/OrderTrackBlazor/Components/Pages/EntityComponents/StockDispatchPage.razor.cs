
using BootstrapBlazor.Components;
using Google.Api;
using Microsoft.AspNetCore.Components;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class StockDispatchPageComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    [Inject]
    public IStockDispatchService? service { get; set; }
    public StockDispatchDTO? DTO { get; set; }
    public List<SelectedItem> StatusItem { get; set; } = new List<SelectedItem>();
    public SelectedItem? SelectedStatusItem { get; set; }
    [Inject]
    public IPackageService PackageService { get; set; }
    public IEnumerable<SelectedItem> PackageSizes { get; set; } = new List<SelectedItem>();
    public Task OnItemChanged(SelectedItem item)
    {
      if (DTO != null && Enum.TryParse<EnumDispatchStatus>(item.Value, out var result))
      {
        DTO.Status = result;
      }
      return Task.CompletedTask;
    }
    public override async Task<bool> SaveFunction()
    {
      if (Id == null && DTO.Id <= 0)
      {
        return await service.Create(DTO);
      }
      else
      {
        return await service.Update(DTO);
      }
      return true;
    }
    public async Task Refresh()
    {
      DTO = Id == null ? await service.GetCreateDTO() : await service.FindDTO(Id.Value);
      StatusItem.Add(new SelectedItem("", "selected"));

      foreach (int i in Enum.GetValues(typeof(EnumDispatchStatus)))
      {
        StatusItem.Add(new SelectedItem(i.ToString(), ((EnumDispatchStatus)i).ToString()));
      }
      if (DTO != null && DTO.Status.HasValue)
      {
        SelectedStatusItem = StatusItem.Where(b => b.Value == ((int)DTO.Status).ToString()).FirstOrDefault();
      }
      PackageSizes = await PackageService.GetPackageSize();
      StateHasChanged();
    }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();

    }
    public async Task DispatchPackage(long? packageId = null)
    {
      var onsave = new OnSaveDTO();
      var comp = BootstrapDynamicComponent.CreateComponent<StockDispatchPackage>(
          new Dictionary<string, object?>()
          {
            ["DTO"] = packageId == null ?
              await service.GetCreateStockDispatchPackageDTO(Id ?? 0) :
              await service.FindStockDispatchPackageDTO(packageId.Value, Id ?? 0),
            ["ParentDTO"] = DTO,
            ["PackageSizes"] = PackageSizes,
            ["OnSave"] = onsave

          });
      var dotion = new DialogOption()
      {
        Title = packageId == null ? "add package" : "edit package",
        Size = Size.ExtraLarge,
        Component = comp,
        ShowSaveButton = true,
        OnSaveAsync = async () =>
        {
          var result = true;
          if (onsave.OnSaveFunc != null)
          {
            result = await onsave.OnSaveFunc();
            Id = DTO?.Id;
          }
          if (result)
          {

          }
          await Refresh();
          return result;
        }
      };
      await dialogService!.Show(dotion);
    }
  }
}