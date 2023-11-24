namespace OrderTrackBlazor.Services
{
  public interface IDispatchService
  {
    Task<DispatchDetailDTO?> GetNewDispatchDetailDTO(long orderId);
    Task<DispatchDetailDTO?> FindDispatchDetailDTO(long dispatchId);
    Task<bool> CreateDispatch(DispatchDetailDTO dto);
    Task<bool> UpdateDispatch(DispatchDetailDTO dto);
    IQueryable<DispatchDetailDTO> GetDispatch(long orderId);
  }
}
