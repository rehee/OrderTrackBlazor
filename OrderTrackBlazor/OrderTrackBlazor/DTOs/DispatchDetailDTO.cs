namespace OrderTrackBlazor.DTOs
{
  public class DispatchDetailDTO
  {
    public long? Id { get; set; }
    public long? OrderId { get; set; }
    public DateTime? DispatchDate { get; set; }
    public DateTime? IncomeDate { get; set; }
    public EnumDispatchStatus? Status { get; set; }
    public decimal? Income { get; set; }
    public IEnumerable<DispatchDetailItemDTO>? Items { get; set; }
  }

  public class DispatchDetailItemDTO
  {
    public Guid RowId { get; set; } = Guid.NewGuid();
    public long? Id { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int Number { get; set; }
  }
}
