namespace OrderTrackBlazor.Services
{
  public interface IStockService
  {
    IQueryable<StockSummaryDTO> QuerySummary();
    IQueryable<StockListDTO> QueryDetail();
    IQueryable<StockRequireDTO> QueryAvaliable();
    IQueryable<StockRequireSummaryDTO> QueryRequireSummary(long? purchaseId = null, bool showAvaliableOnly = true);

    Task<StockRequireSummaryDTO?> FindRequireSummary(long productionId);

    Task<bool> CreateStockPurchase(StockPurchaseDTO stockPurchase);
    Task<bool> UpdateStockPurchase(StockPurchaseDTO stockPurchase);
    Task<StockPurchaseDTO> FindStockPurchase(long purchaseId);

    Task<bool> UpdateStockOrderItem(IEnumerable<StockRequireDTO> dtos);
  }
}
