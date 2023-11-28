namespace OrderTrackBlazor.DTOs
{
  public class OrderPurchaseSummaryDTO
  {
    public long? Id { get; set; }
    public long? OrderId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? ShopId { get; set; }
    public decimal? Price { get; set; }
    public IEnumerable<OrderPurchaseItemDTO>? Items { get; set; }
    public IEnumerable<OrderPurchaseItemDTO>? OrderItems { get; set; }
    public IEnumerable<long?> ProducItems => Items?.Select(x => x.ProductionId) ?? new List<long?>();
    public IEnumerable<OrderPurchaseItemDTO>? EditOrderItem => Items?.Concat(OrderItems?.Where(b => !ProducItems.Contains(b.ProductionId)) ?? new List<OrderPurchaseItemDTO>());
  }

  public class OrderPurchaseItemDTO
  {
    public Guid RowId { get; set; }
    public long? Id { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int Quantity { get; set; }
  }


}
