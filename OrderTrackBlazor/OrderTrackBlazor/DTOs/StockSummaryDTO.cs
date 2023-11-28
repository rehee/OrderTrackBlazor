namespace OrderTrackBlazor.DTOs
{
  public class StockSummaryDTO
  {
    public long? Id { get; set; }
    public string? Name { get; set; }
    public int? CurrentStock { get; set; }
  }

  public class StockListDTO
  {
    public Guid RowId { get; set; } = Guid.NewGuid();
    public long? Pk { get; set; }
    public long? Id { get; set; }
    public string? Name { get; set; }
    public bool? IsPurchase { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? Number { get; set; }
    public string? Shop { get; set; }
    public string? OrderShortNote { get; set; }
  }
}
