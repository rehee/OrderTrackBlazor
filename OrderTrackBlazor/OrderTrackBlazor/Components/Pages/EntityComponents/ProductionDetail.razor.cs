using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace OrderTrackBlazor.Components.Pages.EntityComponents
{
  public class ProductionDetailComponent : CBase
  {
    [Parameter]
    public long? Id { get; set; }
    public ProductionDTO? Production { get; set; }

    [Inject]
    public IProductionService ProductionService { get; set; }

    [Inject]
    [NotNull]
    public ISelectedItemService? SelectedItemService { get; set; }

    public IEnumerable<SelectedItem> Categories { get; set; }
    public SelectedItem? SelectedCategory { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }

    public async Task Refresh()
    {

      Production = await ProductionService.GetProduction(Id);
      Categories = await SelectedItemService.GetEntitySelection<OrderTrackCategory>();
      SelectedCategory = Categories.FirstOrDefault(b => b.Value == Production?.CategoryId?.ToString());
      StateHasChanged();
    }
    public async Task CategoryChange(SelectedItem item)
    {
      await Task.CompletedTask;
      if (Production == null)
      {
        return;
      }
      if (item == null)
      {
        Production.CategoryId = null;
      }
      if (long.TryParse(item.Value, out var longId))
      {
        Production.CategoryId = longId;
      }
    }
    public override async Task<bool> SaveFunction()
    {
      var result = await ProductionService.SaveChange(Production);
      if (OnSave != null)
      {
        OnSave.ResultValue = Production.NewId;
      }
      return result;

    }
    public Task<bool> OnFileDelete()
    {
      Production.Attachment = null;
      StateHasChanged();
      return Task.FromResult(true);
    }
    public async Task OnFileChange(UploadFile file)
    {
      // 未真正保存文件
      // file.SaveToFile()
      if (file != null && file.File != null)
      {

        var format = file.File.ContentType;
        await file.RequestBase64ImageFileAsync(format, 640, 480, long.MaxValue);
        Production.Attachment = file.PrevUrl;
        filesize = (Production.Attachment.Length / (1024 * 1024)).ToString();
      }
      StateHasChanged();
    }
    public string? filesize { get; set; }
    public string? base64String { get; set; }
  }
}