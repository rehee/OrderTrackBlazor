namespace OrderTrackBlazor.Services
{
  public interface IStockDispatchService
  {
    IQueryable<StockDTO> StockQuery(bool asNoTrack = true);
    IQueryable<StockDispatchDTO> StockDispatchQuery(bool asNoTrack = true);

    Task<StockDispatchDTO?> GetCreateDTO();
    Task<StockDispatchDTO?> FindDTO(long id);

    Task<bool> Create(StockDispatchDTO dto);
    Task<bool> Update(StockDispatchDTO dto);
    Task<bool> Update(StockDispatchPackageDTO dto);

    Task<bool> Create(StockDispatchPackageDTO dto, StockDispatchDTO parentdto);

    Task<StockDispatchPackageDTO> GetCreateStockDispatchPackageDTO(long dispatchId);
    Task<StockDispatchPackageDTO> FindStockDispatchPackageDTO(long packageId, long dispatchId);
  }
}
