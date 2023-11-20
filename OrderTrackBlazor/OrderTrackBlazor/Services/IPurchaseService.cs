namespace OrderTrackBlazor.Services
{
  public interface IPurchaseService
  {
    IQueryable<PurchaseDTO> Query();
    Task<PurchaseDTO?> FindAsync(long? id);
  }
}
