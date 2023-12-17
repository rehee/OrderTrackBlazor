using BootstrapBlazor.Components;

namespace OrderTrackBlazor.Services
{
  public interface IPackageService
  {
    Task<List<SelectedItem>> GetPackageSize();
    Task<List<PackageDetailDTO>> GetAllPackages(long orderId);
    Task<PackageDetailDTO> GetNewPackageDTO(long orderId);
    Task<PackageDetailDTO> GetPackageDTO(long id);
    Task<bool> CreatePackageAsync(PackageDetailDTO dto);
    Task<bool> UpdatePackageAsync(PackageDetailDTO dto);
  }
}
