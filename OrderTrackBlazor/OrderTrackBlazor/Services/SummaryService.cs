﻿
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class SummaryService : ISummaryService
  {
    private readonly IContext context;

    public SummaryService(IContext context)
    {
      this.context = context;
    }
    public IQueryable<SummaryDTO> Query()
    {
      
      return
        from order in context.Query<OrderTrackOrder>(true)

        let orderItem = order.Items.Select(b => new SummaryProductionDTO
        {
          ProductionId = b.ProductionId,
          Required = b.Quantity,
          Purchased = b.Production.PurchaseItems.Sum(b => b.Quantity),
          ProductionName = b.Production.Name,
        })
        select new SummaryDTO
        {
          OrderId = order.Id,
          OrderDate = order.OrderDate,
          Productions = orderItem
        };
    }
  }
}