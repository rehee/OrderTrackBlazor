namespace OrderTrackBlazor.DTOs
{
  public class OnSaveDTO
  {
    public Func<Task<bool>>? OnSaveFunc;
  }
}
