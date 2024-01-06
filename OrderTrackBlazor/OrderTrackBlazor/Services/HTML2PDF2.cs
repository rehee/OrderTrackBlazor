using BootstrapBlazor.Components;
using PuppeteerSharp;

namespace OrderTrackBlazor.Services
{
  class DefaultPdfService2 : IHtml2Pdf
  {
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<byte[]> PdfDataAsync(string url)
    {
      using var browserFetcher = new BrowserFetcher();
      await browserFetcher.DownloadAsync();

      await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions() { Headless = true });
      await using var page = await browser.NewPageAsync();
      await page.GoToAsync(url);

      var content = await page.GetContentAsync();
      return await page.PdfDataAsync();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Stream> PdfStreamAsync(string url)
    {
      using var browserFetcher = new BrowserFetcher();
      await browserFetcher.DownloadAsync();

      await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions() { Headless = true });
      await using var page = await browser.NewPageAsync();
      await page.GoToAsync(url);

      var content = await page.GetContentAsync();
      return await page.PdfStreamAsync();
    }
  }

}
