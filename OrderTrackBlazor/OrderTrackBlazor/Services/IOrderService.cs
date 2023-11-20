namespace OrderTrackBlazor.Services
{
  public interface IOrderService
  {
    IQueryable<OrderDTO> Query();
    Task<OrderDTO?> FindAsync(long? id);
  }
}
