namespace OrderTrackBlazor.Services
{
  public interface IProductionService
  {
    Task<IEnumerable<ProductionDTO>> GetAllProductions();
    Task<ProductionDTO?> GetProduction(long? id);
    Task<bool> SaveChange(ProductionDTO? dto);
  }
}
