namespace OrderTrackBlazor.DTOs
{
  public class DispatchListDTO
  {
    public long? Id { get; set; }
    public DateTime? DispatchDate { get; set; }
    public EnumDispatchStatus? Status { get; set; }
    public decimal? Income { get; set; }
  }
}
