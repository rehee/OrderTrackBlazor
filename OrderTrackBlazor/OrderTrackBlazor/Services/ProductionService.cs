using Microsoft.EntityFrameworkCore;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Services
{
  public class ProductionService : IProductionService
  {
    private readonly IContext context;
    public ProductionService(IContext context)
    {
      this.context = context;
    }

    public async Task<IEnumerable<ProductionDTO>> GetAllProductions()
    {
      return await context.Query<OrderTrackProduction>(true)
        .OrderByDescending(b => b.CategoryId != null)
        .ThenBy(b => b.Category.DisplayOrder)
        .ThenBy(b => b.Category.Name)
        .ThenBy(b => b.Name).Select(b => new ProductionDTO
        {
          Id = b.Id,
          OriginalPrice = b.OriginalPrice,
          ProductionName = b.Name,
          Attachment = b.AttachmentId,
          CategoryId = b.CategoryId,
          CategoryName = b.Category.Name
        }).ToArrayAsync();
    }
    public async Task<ProductionDTO?> GetProduction(long? id)
    {
      if (id == null)
      {
        return new ProductionDTO { };
      }
      return await context.Query<OrderTrackProduction>(true)
        .OrderBy(b => b.Name).Select(b => new ProductionDTO
        {
          Id = b.Id,
          OriginalPrice = b.OriginalPrice,
          ProductionName = b.Name,
          ExtendUrl = b.ExtendUrl,
          Attachment = b.AttachmentId,
          CategoryId = b.CategoryId,
          CategoryName = b.Category.Name
        }).Where(b => b.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> SaveChange(ProductionDTO? dto)
    {
      if (dto == null)
      {
        return true;
      }
      try
      {
        if (dto.IsCreate)
        {
          var entity = new OrderTrackProduction
          {
            Name = dto.ProductionName,
            OriginalPrice = dto.OriginalPrice,
            ExtendUrl = dto.ExtendUrl,
            AttachmentId = dto.Attachment,
            CategoryId = dto.CategoryId
          };
          await context.AddAsync<OrderTrackProduction>(entity);
          var result = (await context.SaveChangesAsync()) >= 0;
          dto.NewId = entity.Id;
          return result;
        }
        else
        {
          var production = await context.Query<OrderTrackProduction>(false).Where(b => b.Id == dto.Id).FirstOrDefaultAsync();
          if (production != null)
          {
            production.Name = dto.ProductionName;
            production.OriginalPrice = dto.OriginalPrice;
            production.ExtendUrl = dto.ExtendUrl;
            production.AttachmentId = dto.Attachment;
            production.CategoryId = dto.CategoryId;
          }
          return (await context.SaveChangesAsync()) >= 0;
        }


      }
      catch (Exception ex)
      {
        return false;
      }

    }
  }
}
