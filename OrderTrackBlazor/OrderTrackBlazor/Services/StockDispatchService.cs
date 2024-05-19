using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;
using ReheeCmf.DTOProcessors;

namespace OrderTrackBlazor.Services
{
  public class StockDispatchService : IStockDispatchService
  {
    private readonly IContext context;

    public StockDispatchService(IContext context)
    {
      this.context = context;
    }

    public IQueryable<StockDTO> StockQuery(bool asNoTrack = true)
    {
      return stockQuery(asNoTrack);
    }
    private IQueryable<StockDTO> stockQuery(bool asNoTrack = true, long? packageId = null)
    {
      return (
        from production in context.Query<OrderTrackProduction>(asNoTrack)
        select new StockDTO
        {
          ProductionId = production.Id,
          ProductionName = production.Name,
          AvaliableStock =
            production.PurchaseItems.Sum(b => b.Quantity) -
            production.DispatchItems.Where(b => packageId == null ? true : b.OrderTrackStockDispatchPackageId != packageId)
            .Sum(b => b.Quantity + b.PackageQuantity),
        }
        )
        .AsSplitQuery()
        .Where(b => b.AvaliableStock > 0)
        .OrderByDescending(b => b.AvaliableStock).ThenBy(b => b.ProductionId);
    }
    public IQueryable<StockDispatchDTO> StockDispatchQuery(bool asNoTrack = true)
    {
      var today = DateTime.UtcNow.Date;
      return (
        from dispatch in context.Query<OrderTrackStockDispatch>(asNoTrack)
        select new StockDispatchDTO
        {
          RowId = Guid.NewGuid(),
          Id = dispatch.Id,
          Status = dispatch.Status,
          DispatchDate = dispatch.DispatchDate == null ? today : dispatch.DispatchDate.Value.Date,
          BriefNote = dispatch.BriefNote,
          Note = dispatch.Note,
        })
        .OrderByDescending(b => b.DispatchDate)
        .ThenByDescending(b => b.Id);
    }

    public async Task<StockDispatchDTO?> GetCreateDTO()
    {
      await Task.CompletedTask;
      return new StockDispatchDTO
      {
        DispatchDate = DateTime.UtcNow
      };
    }
    public async Task<bool> Create(StockDispatchDTO dto)
    {
      await create(dto);
      return true;
    }
    private async Task<OrderTrackStockDispatch> create(StockDispatchDTO dto)
    {
      var record = new OrderTrackStockDispatch
      {
        BriefNote = dto.BriefNote,
        DispatchDate = DateTime.UtcNow,
        Note = dto.Note,
        Income = dto.Income,
        Status = dto.Status,
        CompletedDate = dto.CompletedDate,
        Packages = new List<OrderTrackStockDispatchPackage>(),
      };
      await context.AddAsync(record, CancellationToken.None);
      await context.SaveChangesAsync(null);
      return record;
    }
    public async Task<bool> Create(StockDispatchPackageDTO dto, StockDispatchDTO parentdto)
    {
      OrderTrackStockDispatch? dispatch = null;
      if (parentdto.Id <= 0)
      {
        dispatch = await create(parentdto);
      }
      else
      {
        dispatch = await context.Query<OrderTrackStockDispatch>(false).Where(b => b.Id == parentdto.Id).FirstOrDefaultAsync();
      }
      var package = new OrderTrackStockDispatchPackage
      {
        OrderTrackStockDispatchId = dispatch?.Id,
        Dispatch = dispatch,
        Discribtion = dto.Discribtion,
        BriefDiscribtion = dto.BriefDiscribtion,
        PackagePrice = dto.PackagePrice,
        Number = dto.Number,
        PackageSizeId = dto.PackageSizeId,
        PackageWeight = dto.PackageWeight,
      };
      await context.AddAsync<OrderTrackStockDispatchPackage>(package, CancellationToken.None);
      foreach (var item in dto.Items ?? new List<StockDispatchPackageItemDTO>())
      {
        if (item.Number == 0)
        {
          continue;
        }
        var packageItem = new OrderTrackStockDispatchPackageItem()
        {
          PackageId = package.Id,
          Package = package,
          Number = item.Number,
          ProductionId = item.ProductionId,
        };
        await context.AddAsync<OrderTrackStockDispatchPackageItem>(packageItem, CancellationToken.None);
      }
      foreach (var item in dto.OrderItems ?? new List<OrderShortDTO>())
      {
        if (item.Number == 0)
        {
          continue;
        }
        var orderDispatchItem = new OrderTrackDispatchItem
        {
          ProductionId = item.ProductionId,
          OrderProductionId = item.OrderItemId,
          OrderTrackStockDispatchPackageId = package.Id,
          OrderTrackStockDispatchPackage = package,
          DispatchPrice = item.Price,
          PackageQuantity = item.Number,

        };
        await context.AddAsync<OrderTrackDispatchItem>(orderDispatchItem, CancellationToken.None);
      }
      await context.SaveChangesAsync(null);
      parentdto.Id = dispatch.Id;
      return true;
    }
    public async Task<StockDispatchDTO?> FindDTO(long id)
    {
      var query =
        from record in context.Query<OrderTrackStockDispatch>(true).Where(b => b.Id == id).AsSplitQuery()
        select new StockDispatchDTO
        {
          Id = record.Id,
          BriefNote = record.BriefNote,
          DispatchDate = record.DispatchDate,
          Note = record.Note,
          Income = record.Income,
          Status = record.Status,
          CompletedDate = record.CompletedDate,
          Packages = record.Packages.Select(b => new StockDispatchPackageDTO
          {
            Id = b.Id,

            BriefDiscribtion = b.BriefDiscribtion,
            Discribtion = b.Discribtion,
            Number = b.Number == null ? 0 : b.Number.Value,
            PackagePrice = b.PackagePrice,
            PackageSizeId = b.PackageSizeId,
            PackageSizeName = b.PackageSize != null ? b.PackageSize.Name : "",
            PackageWeight = b.PackageWeight,
            StockDispatchId = b.OrderTrackStockDispatchId,
            PackageItem = b.Items.Select(p => new StockPackageItemDTO
            {
              Number = p.Number,
              ProductionName = p.Production.Name,
            }),
            Items = b.OrderItems.Select(o => new StockDispatchPackageItemDTO
            {
              ProductionId = o.ProductionId,
              Number = o.PackageQuantity,
              Price = o.OrderProduction.OrderPrice == null ?
                o.Production.OriginalPrice : 
                o.OrderProduction.OrderPrice,
              ProductionName = o.Production.Name,
              OrderTime = o.OrderProduction.Order.OrderDate
            })
          })
        };
      return await query.FirstOrDefaultAsync();
    }
    public async Task<StockDispatchDTO[]> FindDTO(IEnumerable<long> ids)
    {
      var queryId = ids.ToArray();
      var query =
        from record in context.Query<OrderTrackStockDispatch>(true).Where(b => queryId.Contains(b.Id)).AsSplitQuery()
        select new StockDispatchDTO
        {
          Id = record.Id,
          BriefNote = record.BriefNote,
          DispatchDate = record.DispatchDate,
          Note = record.Note,
          Income = record.Income,
          Status = record.Status,
          CompletedDate = record.CompletedDate,
          Packages = record.Packages.Select(b => new StockDispatchPackageDTO
          {
            Id = b.Id,

            BriefDiscribtion = b.BriefDiscribtion,
            Discribtion = b.Discribtion,
            Number = b.Number == null ? 0 : b.Number.Value,
            PackagePrice = b.PackagePrice,
            PackageSizeId = b.PackageSizeId,
            PackageSizeName = b.PackageSize != null ? b.PackageSize.Name : "",
            PackageWeight = b.PackageWeight,
            StockDispatchId = b.OrderTrackStockDispatchId,
            PackageItem = b.Items.Select(p => new StockPackageItemDTO
            {
              Number = p.Number,
              ProductionName = p.Production.Name,
            }),
            Items = b.OrderItems.Select(o => new StockDispatchPackageItemDTO
            {
              ProductionId = o.ProductionId,
              Number = o.PackageQuantity,
              Price = o.OrderProduction.OrderPrice == null ? o.OrderProduction.Production.OriginalPrice : o.OrderProduction.OrderPrice,
              ProductionName = o.Production.Name,
              OrderTime = o.OrderProduction.Order.OrderDate
            })
          })
        };
      return await query.ToArrayAsync();
    }
    public async Task<bool> Update(StockDispatchDTO dto)
    {
      var record = await context.Query<OrderTrackStockDispatch>(false).Where(b => b.Id == dto.Id).FirstOrDefaultAsync();

      record.BriefNote = dto?.BriefNote;
      record.DispatchDate = dto?.DispatchDate?.Date;
      record.Note = dto?.Note;
      record.Income = dto?.Income;
      record.Status = dto?.Status;
      record.CompletedDate = dto?.CompletedDate?.Date;

      await context.SaveChangesAsync(null);
      return true;
    }
    public async Task<bool> Update(StockDispatchPackageDTO dto)
    {
      var package = await context.Query<OrderTrackStockDispatchPackage>(false).Where(b => b.Id == dto.Id).FirstOrDefaultAsync();

      package.Discribtion = dto.Discribtion;
      package.BriefDiscribtion = dto.BriefDiscribtion;
      package.PackagePrice = dto.PackagePrice;
      package.Number = dto.Number;
      package.PackageSizeId = dto.PackageSizeId;
      package.PackageWeight = dto.PackageWeight;
      await context.SaveChangesAsync(null);

      foreach (var item in dto.Items ?? new List<StockDispatchPackageItemDTO>())
      {
        if (item.Id <= 0)
        {
          if (item.Number <= 0)
          {
            continue;
          }
          var packageItem = new OrderTrackStockDispatchPackageItem()
          {
            PackageId = package.Id,
            Package = package,
            Number = item.Number,
            ProductionId = item.ProductionId,
          };
          await context.AddAsync<OrderTrackStockDispatchPackageItem>(packageItem, CancellationToken.None);
        }
        else
        {
          var packageItem = await context.Query<OrderTrackStockDispatchPackageItem>(false).Where(b => b.Id == item.Id).FirstOrDefaultAsync();
          packageItem.Number = item.Number;
          packageItem.ProductionId = item.ProductionId;
        }

      }

      var allOrderItems = await context.Query<OrderTrackDispatchItem>(false)
          .Where(b => b.OrderTrackStockDispatchPackageId == package.Id).ToArrayAsync();
      var updateIds = new List<long>();
      foreach (var item in dto.OrderItems.ToArray())
      {
        var updateItem = allOrderItems
          .FirstOrDefault(b => b.ProductionId == item.ProductionId && b.OrderProductionId == item.OrderItemId);
        if (updateItem == null)
        {
          await context.AddAsync<OrderTrackDispatchItem>(new OrderTrackDispatchItem
          {
            ProductionId = item.ProductionId,
            OrderProductionId = item.OrderItemId,
            OrderTrackStockDispatchPackageId = package.Id,
            OrderTrackStockDispatchPackage = package,
            DispatchPrice = item.Price,
            PackageQuantity = item.Number,
          }, CancellationToken.None);
          continue;
        }
        if (item.Number == 0)
        {
          context.Delete<OrderTrackDispatchItem>(updateItem);
        }
        else
        {
          updateItem.PackageQuantity = item.Number;
        }
        updateIds.Add(updateItem.Id);
      }
      foreach (var removeItem in allOrderItems.Where(b => !updateIds.Contains(b.Id)))
      {
        context.Delete<OrderTrackDispatchItem>(removeItem);
      }
      await context.SaveChangesAsync(null);
      return true;

    }
    public async Task<StockDispatchPackageDTO> GetCreateStockDispatchPackageDTO(long dispatchId)
    {
      var property = await queryStockDispatchPackageItemDTO();
      return new StockDispatchPackageDTO
      {
        StockDispatchId = dispatchId,
        Items = property,
        PackagePrice = 5,
      };
    }
    public async Task<StockDispatchPackageDTO> FindStockDispatchPackageDTO(long packageId, long dispatchId)
    {
      var property = await queryStockDispatchPackageItemDTO(packageId);
      var record = await (
        from package in context.Query<OrderTrackStockDispatchPackage>(true).Where(b => b.Id == packageId)
        select new StockDispatchPackageDTO
        {
          Id = package.Id,
          BriefDiscribtion = package.BriefDiscribtion,
          Discribtion = package.Discribtion,
          Number = package.Number == null ? 0 : package.Number.Value,
          PackagePrice = package.PackagePrice,
          PackageSizeName = package.PackageSize.Name,
          PackageSizeId = package.PackageSizeId,
          PackageWeight = package.PackageWeight,
        }
        ).FirstOrDefaultAsync();

      record.Items = property;
      return record;
    }
    private async Task<IEnumerable<StockDispatchPackageItemDTO>> queryStockDispatchPackageItemDTO(long? packageId = null)
    {
      var avaliableDTO = await stockQuery(true, packageId).ToListAsync();
      var avaliableProperty = avaliableDTO.Select(b => b.ProductionId);
      StockDispatchPackageItemDTO[] packageItems = packageId == null ? Array.Empty<StockDispatchPackageItemDTO>() :
        await (
        from package in context.Query<OrderTrackStockDispatchPackageItem>(true).Where(b => b.PackageId == packageId)
        select new StockDispatchPackageItemDTO
        {
          Id = package.Id,
          ProductionId = package.ProductionId,
          Number = package.Number,
        }).ToArrayAsync();

      var orderItems = await (
         from item in context.Query<OrderTrackOrderItem>(true)
         .Where(b => avaliableProperty.Contains(b.ProductionId) == true)
         .AsSplitQuery()
           //.Where(b =>
           //   b.Quantity >
           //   b.Production.DispatchItems
           //     .Where(b => packageId == null ? true : b.OrderTrackStockDispatchPackageId != packageId).Sum(b => b.Quantity + b.PackageQuantity))

         select item
           ).ToArrayAsync();
      var property = orderItems.GroupBy(b => b.ProductionId)
        .Select(b => new StockDispatchPackageItemDTO
        {
          ProductionId = b.Key,
          ProductionName = b.FirstOrDefault()?.Production.Name,
          AvaliableStock = avaliableDTO.Where(a => a.ProductionId == b.Key).Select(b => b.AvaliableStock).FirstOrDefault(),
          OrderItems = b.Select(o => new OrderShortDTO
          {
            OrderDate = o.Order.OrderDate,
            OrderItemId = o.Id,
            ProductionId = o.ProductionId,
            ProductionName = o.Production.Name,
            OrderShortNote = o.Order.ShortNote,
            Price = o.OrderPrice ?? o.Production.OriginalPrice,
            ShortNumber = o.Quantity - o.DispatchItems.Where(od => packageId == null ? true : od.OrderTrackStockDispatchPackageId != packageId).Sum(b => b.Quantity + b.PackageQuantity)
          })
        }).ToArray();
      foreach (var p in property)
      {
        var avaliableItem = p.OrderItems.Where(b => b.ShortNumber > 0).ToArray();
        if (avaliableItem.Length > 0)
        {
          p.OrderItems = avaliableItem;
        }
        else
        {
          p.OrderItems = p.OrderItems.OrderByDescending(b => b.OrderDate).Take(1).ToArray();
        }

      }
      foreach (var p in packageItems)
      {
        var p2 = property.FirstOrDefault(b => b.ProductionId == p.ProductionId);
        if (p2 == null)
        {
          continue;
        }
        p2.Id = p.Id;
        p2.Number = p.Number;
      }
      return property;
    }


  }
}
