using Microsoft.EntityFrameworkCore;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using ReheeCmf.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackProduction : BaseOrderTrackEntity
  {
    public string? NormalizationName { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string? ExtendUrl { get; set; }
    public virtual List<OrderTrackPurchaseItem>? PurchaseItems { get; set; }
    public virtual List<OrderTrackDispatchItem>? DispatchItems { get; set; }
    public virtual List<OrderTrackOrderItem>? OrderItems { get; set; }
  }

  [EntityChangeTracker<OrderTrackProduction>]
  public class OrderTrackProductionHandler : OrderTrackEntityHandler<OrderTrackProduction>
  {
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var result = await base.ValidationAsync(ct);
      var validation = new List<ValidationResult>();
      entity.NormalizationName = entity?.Name?.Trim().ToUpper() ?? "";
      var alreadyExist = await context.Query<OrderTrackProduction>(true)
        .AnyAsync(b => b.NormalizationName == entity.NormalizationName && b.Id != entity.Id);
      if (alreadyExist)
      {
        validation.Add(ValidationResultHelper.New("production already existing", nameof(entity.Name)));
      }
      return result.Concat(validation);
    }
    private void updateName()
    {

    }
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);

    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);

    }
  }
}
