using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class OrderService : IOrderService
  {
    private readonly IContext context;
    public OrderService(IContext context)
    {
      this.context = context;
    }

    public IQueryable<OrderDTO> Query()
    {
      return from order in context.Query<OrderTrackOrder>(true)

             select new OrderDTO
             {
               OrderDate = order.OrderDate,
               Id = order.Id,
               ShortNote = order.ShortNote,
               Note = order.Note,
               Productions = order.Items.Select(b =>
                new OrderProductionDTO
                {
                  Id = b.Id,
                  ProductionId = b.ProductionId,
                  Quantity = b.Quantity,
                }).ToList()
             };
    }

    public async Task<OrderDTO?> FindAsync(long? id)
    {
      return await Query().FirstOrDefaultAsync(b => b.Id == id);
    }
  }
}
