
using Google.Api;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class PurchaseService : IPurchaseService
  {
    private readonly IContext context;
    public PurchaseService(IContext context)
    {
      this.context = context;
    }
    public async Task<PurchaseDTO?> FindAsync(long? id)
    {
      return await Query().FirstOrDefaultAsync(b => b.Id == id);
    }

    public IQueryable<PurchaseDTO> Query()
    {
      return
        from purchase in context.Query<OrderTrackPurchaseRecord>(true)

        select new PurchaseDTO
        {
          Id = purchase.Id,
          PurchaseDate = purchase.PurchaseDate,
          CreateDate = purchase.CreateDate,
          ShopId = purchase.ShopId,
          ShopName = purchase.Shop.Name,
          ShopPrice = purchase.Price,
          OrderId = purchase.OrderId,
          ReceiptImage = purchase.ReceiptImage,
          Productions = purchase.Items.Select(b =>
           new OrderProductionDTO
           {
             Id = b.Id,
             ProductionId = b.ProductionId,
             Quantity = b.Quantity,
             CreateDate = b.CreateDate,
           }).ToList()
        };
    }
  }
}
