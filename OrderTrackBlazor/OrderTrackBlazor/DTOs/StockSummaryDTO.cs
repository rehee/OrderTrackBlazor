namespace OrderTrackBlazor.DTOs
{
  public class StockSummaryDTO
  {
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public string? CategoryName { get; set; }
    public int? CurrentStock
    {
      get
      {
        return currentStock;
      }
      set
      {
        currentStock = value;
        if (value.HasValue)
        {
          if (value > 0)
          {
            ColumnColor = BootstrapBlazor.Components.Color.Success;
          }
        }
      }
    }
    public BootstrapBlazor.Components.Color ColumnColor { get; set; }
    private int? currentStock { get; set; }
  }

  public class StockListDTO
  {
    public Guid RowId { get; set; } = Guid.NewGuid();
    public long? Pk { get; set; }
    public long? Id { get; set; }
    public string? Name { get; set; }
    public EnumInOutStatus? IsPurchase { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? Number { get; set; }
    public string? Shop { get; set; }
    public string? OrderShortNote { get; set; }
  }
}
