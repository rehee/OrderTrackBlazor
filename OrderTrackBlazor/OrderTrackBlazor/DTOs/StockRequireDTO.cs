namespace OrderTrackBlazor.DTOs
{
  public class StockRequireDTO
  {
    public long OrderItemId { get; set; }
    public long? OrderId { get; set; }
    public string? OrderNote { get; set; }
    public DateTime? OrderDate { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public long? RecommandShopId { get; set; }
    public string? RecommandShopName { get; set; }
    public long? RecommandShopId2 { get; set; }
    public string? RecommandShopName2 { get; set; }

    public IEnumerable<string?> RecommandShopNames
    {
      get
      {
        return new string?[] { RecommandShopName, RecommandShopName2 };
      }
      set { }
    }

    public string? Note { get; set; }
    public decimal? OrderPrice { get; set; }
    public int RequiredNumber { get; set; }
    public int DispatchNumber { get; set; }
    public int StockNumber { get; set; }
    public int OrderItemPuurchase { get; set; }
    public int Pending => RequiredNumber - DispatchNumber;
  }
  public class StockRequireSummaryDTO
  {
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int RequiredNumber { get; set; }
    public int DispatchNumber { get; set; }
    public int PendingNumber { get; set; }

    public int StockNumber { get; set; }
    public int? PurchaseNumber { get; set; }
    public int? Number { get; set; }
    public string RecommandShop
    {
      get
      {
        return string.Join(", ", RecommandShops);
      }
      set { }
    }
    public string? SelectedShop { get; set; }
    public IEnumerable<string> RecommandShops => Items == null ? Enumerable.Empty<string>() : Items.SelectMany(b => b.RecommandShopNames).Where(b => !String.IsNullOrEmpty(b)).Select(b => b as string).Distinct().OrderBy(b => b);
    public IEnumerable<StockRequireDTO>? Items { get; set; }
  }

  public class StockPurchaseDTO
  {
    public StockPurchaseDTO()
    {

    }
    public StockPurchaseDTO(IEnumerable<StockRequireSummaryDTO> dto, long purchaseId, DateTime? purchaseDate, long? shopId, decimal? price)
    {
      Request = dto;
      PurchaseId = purchaseId;
      PurchaseDate = purchaseDate;
      ShopId = shopId;
      Price = price;
    }
    public long PurchaseId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public long? ShopId { get; set; }
    public decimal? Price { get; set; }
    public IEnumerable<StockRequireSummaryDTO>? Request { get; set; }
  }
}
