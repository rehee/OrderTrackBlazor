using Microsoft.EntityFrameworkCore;
using ReheeCmf.Components.ChangeComponents;
using ReheeCmf.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrackBlazor.Entities
{
  public class OrderTrackDispatchRecord : BaseOrderTrackEntity
  {
    [ForeignKey(nameof(OrderTrackOrder))]
    public long? OrderTrackOrderId { get; set; }
    public virtual OrderTrackOrder? Order { get; set; }

    public EnumDispatchStatus Status { get; set; }
    public DateTime? DispatchDate { get; set; }
    public decimal? Income { get; set; }
    public DateTime? IncomeDate { get; set; }
    public DateTime? SoftDeleteUntil { get; set; }
    public string? Note { get; set; }
    public int? PackageNumber { get; set; }
    public virtual List<OrderTrackDispatchItem>? Items { get; set; }
    public virtual List<OrderTrackDispatchPackage>? PackageRecords { get; set; }

  }
  [EntityChangeTracker<OrderTrackDispatchRecord>]
  public class OrderTrackDispatchRecordEntityHandler : OrderTrackEntityHandler<OrderTrackDispatchRecord>
  {
    public override async Task BeforeCreateAsync(CancellationToken ct = default)
    {
      await base.BeforeCreateAsync(ct);
      setSoftDelete();
    }
    public override async Task BeforeUpdateAsync(EntityChanges[] propertyChange, CancellationToken ct = default)
    {
      await base.BeforeUpdateAsync(propertyChange, ct);
      setSoftDelete();
    }

    public override Task BeforeDeleteAsync(CancellationToken ct = default)
    {
      if (entity.Items?.Any() == true)
      {
        foreach (var e in entity.Items)
        {
          context.Delete<OrderTrackDispatchItem>(e);
        }
      }
      return Task.CompletedTask;
    }

    private void setSoftDelete()
    {
      if (entity.Status == EnumDispatchStatus.Error)
      {
        entity.SoftDeleteUntil = DateTime.UtcNow.AddMinutes(15);
      }
      else
      {
        entity.SoftDeleteUntil = null;
      }
    }

    public override async Task AfterCreateAsync(CancellationToken ct = default)
    {
      await base.AfterCreateAsync(ct);
      var items = await context.Query<OrderTrackDispatchItem>(true)
        .Where(b => b.DispatchRecordId == entity.Id)
        .Select(b => b.OrderProduction).DistinctBy(b => b.Id).ToArrayAsync();
      foreach (var item in items)
      {
        if (item == null)
        {
          continue;
        }
        await item.OverDeliveredCheck(context);
      }


    }
    public override async Task AfterUpdateAsync(CancellationToken ct = default)
    {
      await base.AfterUpdateAsync(ct);
      var items = await context.Query<OrderTrackDispatchItem>(true)
        .Where(b => b.DispatchRecordId == entity.Id)
        .Select(b => b.OrderProduction).DistinctBy(b => b.Id).ToArrayAsync();
      foreach (var item in items)
      {
        if (item == null)
        {
          continue;
        }
        await item.OverDeliveredCheck(context);
      }
    }
    public override async Task AfterDeleteAsync(CancellationToken ct = default)
    {
      await base.AfterDeleteAsync(ct);
      var items = await context.Query<OrderTrackDispatchItem>(true)
        .Where(b => b.DispatchRecordId == entity.Id)
        .Select(b => b.OrderProduction).DistinctBy(b => b.Id).ToArrayAsync();
      foreach (var item in items)
      {
        if (item == null)
        {
          continue;
        }
        await item.OverDeliveredCheck(context);
      }
    }
  }
}
