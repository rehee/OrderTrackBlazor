using System.Linq;

namespace OrderTrackBlazor.DTOs
{
  public class OrderPurchaseSummaryDTO
  {
    public long? Id { get; set; }
    public long? OrderId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? ShopId { get; set; }
    public string? ShopName { get; set; }
    public decimal? Price { get; set; }
    public IEnumerable<OrderPurchaseItemDTO>? Items { get; set; }
    public IEnumerable<OrderPurchaseItemDTO>? OrderItems { get; set; }
    public IEnumerable<long?> ProducItems => Items?.Select(x => x.ProductionId) ?? new List<long?>();
    public IEnumerable<OrderPurchaseItemDTO>? EditOrderItem
    {
      get
      {
        if (Items == null)
        {
          return OrderItems ?? new List<OrderPurchaseItemDTO>();
        }
        var ids = Items.Select(b => b.ProductionId).ToArray();
        return Items?.Concat(OrderItems?.Where(b => ids.Contains(b.ProductionId) != true) ?? new List<OrderPurchaseItemDTO>());
      }
    }
  }

  public class OrderPurchaseItemDTO
  {
    public Guid RowId { get; set; }
    public long? Id { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int? Quantity { get; set; }

    public int? QuantityDisplay
    {
      get
      {
        if (Quantity == 0)
        {
          return null;
        }
        return Quantity;
      }
      set
      {
        Quantity = value ?? 0;
      }
    }
  }


}
