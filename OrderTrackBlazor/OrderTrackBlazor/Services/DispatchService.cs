using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
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
        IncomeDate = dto.IncomeDate,
      };
      await context.AddAsync<OrderTrackDispatchRecord>(record, CancellationToken.None);
      foreach (var item in dto.Items ?? new List<DispatchDetailItemDTO>())
      {
        var recordItem = new OrderTrackDispatchItem
        {
          PurchaseRecordId = record.Id,
          DispatchRecord = record,
          ProductionId = item.ProductionId,
          Quantity = item.Number,
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
      dispatch.IncomeDate = dto.IncomeDate;
      if (dispatch.Items?.Any() == true)
      {
        foreach (var item in dispatch.Items)
        {
          var patchItem = dto.Items?.FirstOrDefault(b => b.Id == item.Id);
          if (patchItem == null)
          {
            continue;
          }
          item.Quantity = patchItem.Number;
        }
      }
      await context.SaveChangesAsync(null);
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
          OrderId = dispatch.OrderTrackOrderId,
          Status = dispatch.Status,
          Income = dispatch.Income,
          Items = dispatch.Items.Select(b => new DispatchDetailItemDTO()
          {
            Id = b.Id,
            Number = b.Quantity,
            ProductionId = b.ProductionId,
            ProductionName = b.Production.Name
          }),
        };
    }

    public async Task<DispatchDetailDTO?> GetNewDispatchDetailDTO(long orderId)
    {

      var order = context.Query<OrderTrackOrder>(true).Where(b => b.Id == orderId).FirstOrDefault();
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
        Items = order.Items?.Select(b => new DispatchDetailItemDTO
        {
          Number = 0,
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name
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
        Id = dispatch.Id,
        DispatchDate = dispatch.DispatchDate,
        Income = dispatch.Income,
        Status = dispatch.Status,
        IncomeDate = dispatch.IncomeDate,
        Items = dispatch.Items?.Select(b => new DispatchDetailItemDTO
        {
          Id = b.Id,
          Number = b.Quantity,
          ProductionId = b.ProductionId,
          ProductionName = b.Production?.Name
        }).ToList(),
      };
    }

  }
}
