namespace OrderTrackBlazor.DTOs
{
  public class OnSaveDTO
  {
    public Func<Task<bool>>? OnSaveFunc;
    public object? ResultValue { get; set; }
  }
}
