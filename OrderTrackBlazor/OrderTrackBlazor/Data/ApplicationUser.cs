using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Identity;
using ReheeCmf.Commons;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.ContextModule.Entities;
using ReheeCmf.Entities;

namespace OrderTrackBlazor.Data
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class ApplicationUser : ReheeCmfBaseUser
  {
    public ApplicationUser()
    {
    }
  }

  [EntityChangeTracker<ApplicationUser>]
  public class BaseOrderEntityHandler : OrderTrackEntityHandler<ApplicationUser>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      StatusException.Throw();
    }
  }

}
