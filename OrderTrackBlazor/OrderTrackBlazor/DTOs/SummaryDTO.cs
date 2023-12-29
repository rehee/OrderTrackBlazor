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
    public int Shortage { get => Required - Dispatched; set { } }
    public int NeedToBuy { get => Shortage > 0 ? (Shortage - Purchased) > 0 ? Shortage - Purchased : 0 : 0; set { } }
    public string? RecommandShopName { get; set; }
    public string? RecommandShopName2 { get; set; }
    public string? RecommandShopNameDisplay
    {
      get
      {
        return (RecommandShopName+ " " + RecommandShopName2).Trim();
      }
      set { }
    }
    public string? Note { get; set; }
    public int Avaliable { get; set; }
  }
}
