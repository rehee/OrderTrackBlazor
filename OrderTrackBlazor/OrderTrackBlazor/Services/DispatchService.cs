using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class DispatchService : IDispatchService
  {
    private readonly IContext context;

    public DispatchService(IContext context)
    {
      this.context = context;
    }

    public async Task<bool> CreateDispatch(DispatchDetailDTO dto)
    {
      var record = new OrderTrackDispatchRecord
      {
        OrderTrackOrderId = dto.OrderId,
        Status = dto.Status ?? EnumDispatchStatus.Packing,
        DispatchDate = dto.DispatchDate,
        Income = dto.Income,
        Note = dto.Note,
        PackageNumber = dto.PackageNumber,
        IncomeDate = dto.IncomeDate,
      };
      await context.AddAsync<OrderTrackDispatchRecord>(record, CancellationToken.None);
      foreach (var item in dto.Items.DistinctBy(b => b.ProductionId) ?? new List<DispatchDetailItemDTO>())
      {
        if (item.CalculateNumber == 0)
        {
          continue;
        }
        var recordItem = new OrderTrackDispatchItem
        {
          DispatchRecordId = record.Id,
          DispatchRecord = record,
          ProductionId = item.ProductionId,
          Quantity = item.Number,
          PackageQuantity = item.NumberFromPackage,
          OrderProductionId = item.OrderProductionId,
          DispatchPrice = item.DispatchPrice
        };
        await context.AddAsync<OrderTrackDispatchItem>(recordItem, CancellationToken.None);
      }
      foreach (var item in dto.SourcePackages.DistinctBy(b => b.PackageId) ?? new List<PackageDetailDTO>())
      {
        var dispatchPackage = new OrderTrackDispatchPackage
        {
          RecordId = record.Id,
          Record = record,
          PackageId = item.PackageId,
          BriefDiscribtion = item.BriefDiscribtionFromDispatch,
          Discribtion = item.DiscribtionFromDispatch,
          Number = item.Number,
        };
        await context.AddAsync<OrderTrackDispatchPackage>(dispatchPackage, CancellationToken.None);
      }
      await context.SaveChangesAsync(null);
      return true;
    }

    public async Task<bool> UpdateDispatch(DispatchDetailDTO dto)
    {
      var dispatch = context.Query<OrderTrackDispatchRecord>(false).Where(b => b.Id == dto.Id).FirstOrDefault();
      if (dispatch == null)
      {
        return false;
      }
      dispatch.Status = dto.Status ?? EnumDispatchStatus.Packing;
      dispatch.DispatchDate = dto.DispatchDate;
      dispatch.Income = dto.Income;
      dispatch.PackageNumber = dto.PackageNumber;
      dispatch.Note = dto.Note;
      dispatch.IncomeDate = dto.IncomeDate;
      if (dto.EditItems?.Any() == true)
      {
        foreach (var item in dto.EditItems)
        {
          if (item.Id == null || item.Id <= 0)
          {
            if (item.CalculateNumber == 0)
            {
              continue;
            }
            await context.AddAsync<OrderTrackDispatchItem>(new OrderTrackDispatchItem()
            {
              DispatchRecordId = dispatch.Id,
              DispatchRecord = dispatch,
              ProductionId = item.ProductionId,
              Quantity = item.Number,
              PackageQuantity = item.NumberFromPackage,
              OrderProductionId = item.OrderProductionId,
              DispatchPrice = item.DispatchPrice

            }, CancellationToken.None);
          }
          else
          {
            var record = dispatch.Items.FirstOrDefault(b => b.Id == item.Id);
            if (record == null)
            {
              continue;
            }
            record.Quantity = item.Number;
            record.DispatchPrice = item.DispatchPrice;
            record.ProductionId = item.ProductionId;
            record.OrderProductionId = item.OrderProductionId;
            record.PackageQuantity = item.NumberFromPackage;
          }
        }
        foreach (var item in dto.SourcePackages.DistinctBy(b => b.PackageId) ?? new List<PackageDetailDTO>())
        {
          if (item.Id <= 0)
          {
            if (item.Number == 0)
            {
              continue;
            }
            var dispatchPackage = new OrderTrackDispatchPackage
            {
              RecordId = dispatch.Id,
              Record = dispatch,
              PackageId = item.PackageId,
              BriefDiscribtion = item.BriefDiscribtionFromDispatch,
              Discribtion = item.DiscribtionFromDispatch,
              Number = item.Number,
            };
            await context.AddAsync<OrderTrackDispatchPackage>(dispatchPackage, CancellationToken.None);
          }
          else
          {
            var record = dispatch.PackageRecords.FirstOrDefault(b => b.Id == item.Id);
            if (record == null)
            {
              continue;
            }
            record.Number = item.Number;
            record.BriefDiscribtion = item.BriefDiscribtionFromDispatch;
            record.Discribtion = item.DiscribtionFromDispatch;
          }

        }
      }
      try
      {
        await context.SaveChangesAsync(null);
      }
      catch (Exception ex)
      {
        var a = 1;
      }

      return true;
    }

    public IQueryable<DispatchDetailDTO> GetDispatch(long orderId)
    {

      return
        from dispatch in context.Query<OrderTrackDispatchRecord>(true).Where(b => b.OrderTrackOrderId == orderId)
        select new DispatchDetailDTO
        {
          Id = dispatch.Id,
          DispatchDate = dispatch.DispatchDate,
          IncomeDate = dispatch.IncomeDate,
          CreateDate = dispatch.CreateDate,
          PackageNumber = dispatch.PackageNumber,
          OrderId = dispatch.OrderTrackOrderId,
          Note = dispatch.Note,
          Status = dispatch.Status,
          Income = dispatch.Income,
          Items = dispatch.Items.Select(b => new DispatchDetailItemDTO()
          {
            Id = b.Id,
            RowId = Guid.NewGuid(),
            Number = b.Quantity,
            NumberFromPackage = b.PackageQuantity,
            DispatchPrice = b.DispatchPrice != null ? b.DispatchPrice : b.OrderProduction != null ? b.OrderProduction.OrderPrice != null ? b.OrderProduction.OrderPrice : b.Production.OriginalPrice : b.Production.OriginalPrice,
            ProductionId = b.ProductionId,
            ProductionName = b.Production.Name,
            OrderProductionId = b.OrderProductionId
          }),
          Packages = dispatch.PackageRecords.Select(b => new PackageDetailDTO()
          {
            Id = b.Id,
            PackageId = b.PackageId,
            Number = b.Number,
            BriefDiscribtion = b.Package.BriefDiscribtion,
            Discribtion = b.Package.Discribtion,
            BriefDiscribtionFromDispatch = b.BriefDiscribtion,
            DiscribtionFromDispatch = b.Discribtion
          })

        };
    }

    public async Task<DispatchDetailDTO?> GetNewDispatchDetailDTO(long orderId)
    {

      var order = context.Query<OrderTrackOrder>(true).Include(b => b.Items).ThenInclude(b => b.Production).Where(b => b.Id == orderId).FirstOrDefault();
      if (order == null)
      {
        return new DispatchDetailDTO()
        {
          Items = new List<DispatchDetailItemDTO>()
        };
      }
      var oPackage = order.Packages.Where(b => b.Confirmed == true)
        .Select(b => new PackageDetailDTO
        {
          RowId = Guid.NewGuid(),
          PackageId = b.Id,
          BriefDiscribtion = b.BriefDiscribtion,
          OrderItems = b.Items.Select(bi => new PackageItemDTO
          {
            ProductionId = bi.ProductionId,
            Number = bi.Number,
          }).Where(b => b.Number > 0).ToList(),
        }).ToList();
      return new DispatchDetailDTO
      {
        OrderId = orderId,
        DispatchDate = DateTime.UtcNow,
        Items = order.Items?.Select(b => new DispatchDetailItemDTO
        {
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name,
          OrderProductionId = b.Id,
          DispatchPrice = b.OrderPrice != null ? b.OrderPrice : b.Production?.OriginalPrice
        }).ToList(),
        OrderPackages = oPackage,
      };
    }
    public async Task<DispatchDetailDTO?> FindDispatchDetailDTO(long dispatchId)
    {
      var dispatch = context.Query<OrderTrackDispatchRecord>(true).Where(b => b.Id == dispatchId).FirstOrDefault();
      if (dispatch == null)
      {
        return new DispatchDetailDTO()
        {
          Items = new List<DispatchDetailItemDTO>()
        };
      }
      var OrderItems = dispatch.Order.Items.Select(b => new DispatchDetailItemDTO
      {
        RowId = Guid.NewGuid(),
        ProductionId = b.ProductionId,
        ProductionName = b.Production.Name,
        OrderProductionId = b.Id,
        DispatchPrice = b.OrderPrice != null ? b.OrderPrice : b.Production.OriginalPrice
      }).ToList();
      var orderPackage = dispatch.Order.Packages.Where(b => b.Confirmed == true)
        .Select(b => new PackageDetailDTO
        {
          RowId = Guid.NewGuid(),
          PackageId = b.Id,
          BriefDiscribtion = b.BriefDiscribtion,
          OrderItems = b.Items.Select(bi => new PackageItemDTO
          {
            ProductionId = bi.ProductionId,
            Number = bi.Number,
          }).Where(b => b.Number > 0).ToList(),
        }).ToList();
      var dispatchPackage = dispatch.PackageRecords.Select(b => new PackageDetailDTO
      {
        Id = b.Id,
        PackageId = b.PackageId,
        Number = b.Number,
        BriefDiscribtion = b.Package.BriefDiscribtion,
        Discribtion = b.Package.Discribtion,
        BriefDiscribtionFromDispatch = b.BriefDiscribtion,
        DiscribtionFromDispatch = b.Discribtion,
        Items = b.Package.Items.Select(p => new PackageItemDTO
        {
          ProductionId = p.ProductionId,
          Number = p.Number,
        }).ToList()
      }).ToList();
      return new DispatchDetailDTO
      {
        OrderId = dispatch.OrderTrackOrderId,
        Note = dispatch.Note,
        PackageNumber = dispatch.PackageNumber,
        Id = dispatch.Id,
        DispatchDate = dispatch.DispatchDate,
        Income = dispatch.Income,
        Status = dispatch.Status,
        IncomeDate = dispatch.IncomeDate,
        Items = dispatch.Items?.Select(b => new DispatchDetailItemDTO
        {
          Id = b.Id,
          RowId = Guid.NewGuid(),
          OrderProductionId = b.OrderProductionId,
          Number = b.Quantity,
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name,
          DispatchPrice = b.DispatchPrice != null ? b.DispatchPrice : b.OrderProduction != null ? b.OrderProduction.OrderPrice != null ? b.OrderProduction.OrderPrice : b.Production.OriginalPrice : b.Production.OriginalPrice
        }).ToList(),
        OrderItems = OrderItems,
        OrderPackages = orderPackage,
        Packages = dispatchPackage
      };
    }

  }
}
