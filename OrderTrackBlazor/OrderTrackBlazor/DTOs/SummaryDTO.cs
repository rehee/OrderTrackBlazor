namespace OrderTrackBlazor.DTOs
{
  public class SummaryDTO
  {
    public long? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? OrderCreateDate { get; set; }
    public string? ShortNote { get; set; }
    public IEnumerable<SummaryProductionDTO> Productions { get; set; }
  }

  public class SummaryProductionDTO
  {
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int Required { get; set; }
    public int Purchased { get; set; }
    public int Dispatched { get; set; }
    public int NeedToBuy { get => (Required - Purchased - Dispatched) > 0 ? (Required - Purchased - Dispatched) : 0; set { } }
    public string? RecommandShopName { get; set; }
    public string? Note { get; set; }
  }
}
