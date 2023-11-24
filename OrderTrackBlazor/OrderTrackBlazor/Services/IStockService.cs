namespace OrderTrackBlazor.Services
{
  public interface IStockService
  {
    IQueryable<StockSummaryDTO> QuerySummary();
    IQueryable<StockListDTO> QueryDetail();
  }
}
