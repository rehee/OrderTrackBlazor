
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using OrderTrackBlazor.Consts;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.Helpers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Components.Pages
{
  public class StockDispatchComponent : CBase
  {

    [Inject]
    public IStockDispatchService? stockDispatchService { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      await Refresh();
    }
    public IEnumerable<StockDispatchDTO> DispatchDTO { get; set; } = Enumerable.Empty<StockDispatchDTO>();
    public async Task Refresh()
    {
      DispatchDTO = await stockDispatchService.StockDispatchQuery().ToArrayAsync();
      StateHasChanged();
    }


    public async Task OpenStockDispatch(long? id = null)
    {
      await dialogService.ShowComponent<StockDispatchPage>(
        new Dictionary<string, object?>
        {
          ["Id"] = id
        },
        "",
        true,
        async save => await Refresh()
        );
    }

    public async Task ViewStockDispatch(long id)
    {

      await dialogService.ShowComponent<ViewStockDispatchPage>(
        new Dictionary<string, object?>
        {
          ["Id"] = id
        });
    }
    public string SelectIds => String.Join(",", DispatchDTO.Where(b => b.Selected).Select(b => b.Id));
    [Inject]
    public IComponentHtmlRenderer htmlRenderer { get; set; }
    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHostEnvironment { get; set; }

    [Inject]
    [NotNull]
    private IHtml2Pdf? Html2PdfService { get; set; }
    [Inject]
    [NotNull]
    private DownloadService? downloadService { get; set; }
    public async Task ViewStockDispatchs()
    {
      var ids = DispatchDTO.Where(b => b.Selected).Select(b => b.Id).ToArray();
      if (ids.Any() != true)
      {
        return;
      }
      var html = await htmlRenderer.RenderAsync<ViewStockDispatchPages>(new Dictionary<string, object?>()
      {
        ["Ids"] = ids
      });
      var fileName = $"pdf/c046956e-0edc-495d-8e51-8c7241b29f5d.html";
      var filePath = System.IO.Path.Combine(WebHostEnvironment.WebRootPath, fileName);
      //await using var writer = File.CreateText(filePath);
      //await writer.WriteLineAsync(html);
      //await writer.FlushAsync();
      //writer.Close();
      var url = $"{nm.BaseUri}{fileName}";
      var data = await Html2PdfService.PdfDataAsync(url);
      await downloadService.DownloadFromByteArrayAsync("table.pdf", data);
     

    }
  }

}