namespace OrderTrackBlazor.DTOs
{
  public class SummaryDTO
  {
    public long? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? ShortNote { get; set; }
    public IEnumerable<SummaryProductionDTO> Productions { get; set; }
  }

  public class SummaryProductionDTO
  {
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int Required { get; set; }
    public int Purchased { get; set; }

    public int NeedToBuy { get => (Required - Purchased) > 0 ? (Required - Purchased) : 0; set { } }

  }
}
