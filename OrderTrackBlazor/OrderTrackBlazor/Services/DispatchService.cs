using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using ReheeCmf.Contexts;
using System.Xml.XPath;

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
      foreach (var item in dto.Items ?? new List<DispatchDetailItemDTO>())
      {
        if (item.Number == 0)
        {
          continue;
        }
        var recordItem = new OrderTrackDispatchItem
        {
          DispatchRecordId = record.Id,
          DispatchRecord = record,
          ProductionId = item.ProductionId,
          Quantity = item.Number,
          OrderProductionId = item.OrderProductionId,
          DispatchPrice = item.DispatchPrice
        };
        await context.AddAsync<OrderTrackDispatchItem>(recordItem, CancellationToken.None);
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
            if (item.Number == 0)
            {
              continue;
            }
            await context.AddAsync<OrderTrackDispatchItem>(new OrderTrackDispatchItem()
            {
              DispatchRecordId = dispatch.Id,
              DispatchRecord = dispatch,
              ProductionId = item.ProductionId,
              Quantity = item.Number,
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
            DispatchPrice = b.DispatchPrice != null ? b.DispatchPrice : b.OrderProduction != null ? b.OrderProduction.OrderPrice != null ? b.OrderProduction.OrderPrice : b.Production.OriginalPrice : b.Production.OriginalPrice,
            ProductionId = b.ProductionId,
            ProductionName = b.Production.Name,
            OrderProductionId = b.OrderProductionId
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

      return new DispatchDetailDTO
      {
        OrderId = orderId,
        DispatchDate = DateTime.UtcNow,
        Items = order.Items?.Select(b => new DispatchDetailItemDTO
        {
          Number = 0,
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name,
          OrderProductionId = b.Id,
          DispatchPrice = b.OrderPrice != null ? b.OrderPrice : b.Production?.OriginalPrice
        }).ToList(),
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
        OrderItems = dispatch.Order.Items.Select(b => new DispatchDetailItemDTO
        {
          Number = 0,
          ProductionId = b.ProductionId,
          ProductionName = b.Production.Name,
          OrderProductionId = b.Id,
          DispatchPrice = b.OrderPrice != null ? b.OrderPrice : b.Production.OriginalPrice
        })
      };
    }

  }
}
