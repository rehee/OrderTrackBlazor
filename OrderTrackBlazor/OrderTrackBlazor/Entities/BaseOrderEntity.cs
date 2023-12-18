using Google.Api;
using OrderTrackBlazor.Data;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Handlers.EntityChangeHandlers;

namespace OrderTrackBlazor.Entities
{
  public class BaseOrderTrackEntity : EntityBase<long>, ISelectedItemEntity
  {
    public DateTime? CreateDate { get; set; }
    public string? CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? UpdateUserId { get; set; }
    public string? Name { get; set; }
    public int DisplayOrder { get; set; }
  }


  [EntityChangeTracker<BaseOrderTrackEntity>]
  public class BaseOrderEntityHandler : OrderTrackEntityHandler<BaseOrderTrackEntity>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      entity!.CreateDate = DateTime.UtcNow;
      entity!.CreateUserId = context?.User?.UserId;
    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      entity!.UpdateDate = DateTime.UtcNow;
      entity!.UpdateUserId = context?.User?.UserId;
    }
  }

  public abstract class OrderTrackEntityHandler<T> : EntityChangeHandler<T> where T : class
  {
    protected ApplicationDbContext ThisContext
    {
      get
      {
        return (context!.Context as ApplicationDbContext)!;
      }
    }
  }
}
