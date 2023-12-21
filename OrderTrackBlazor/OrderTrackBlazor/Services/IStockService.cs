namespace OrderTrackBlazor.Services
{
  public interface IStockService
  {
    IQueryable<StockSummaryDTO> QuerySummary();
    IQueryable<StockListDTO> QueryDetail();
    IQueryable<StockRequireDTO> QueryAvaliable();
    IQueryable<StockRequireSummaryDTO> QueryRequireSummary(long? purchaseId = null);

    Task<bool> CreateStockPurchase(StockPurchaseDTO stockPurchase);
    Task<bool> UpdateStockPurchase(StockPurchaseDTO stockPurchase);
    Task<StockPurchaseDTO> FindStockPurchase(long purchaseId);
  }
}
