namespace OrderTrackBlazor.Services
{
  public interface ISummaryService
  {
    IQueryable<SummaryDTO> Query();
    IQueryable<OrderPurchaseSummaryDTO> GetOrderSummary(long orderId);
    Task<OrderPurchaseSummaryDTO> NewOrderPurchaseSummaryDTO(long orderId);
    Task<OrderPurchaseSummaryDTO?> FindOrderPurchaseSummaryDTO(long purchaseId);

    Task<bool> CreateOrderPurchase(OrderPurchaseSummaryDTO dto);
    Task<bool> UpdateOrderPurchase(OrderPurchaseSummaryDTO dto);
  }
}
