
using BootstrapBlazor.Components;
using Google.Api;
using Microsoft.EntityFrameworkCore;
using ReheeCmf.Contexts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderTrackBlazor.Services
{
  public class PackageService : IPackageService
  {
    private readonly IContext context;

    public PackageService(IContext context)
    {
      this.context = context;
    }
    public async Task<bool> CreatePackageAsync(PackageDetailDTO dto)
    {
      var record = new OrderTrackPackage
      {
        OrderId = dto.OrderId,
        Confirmed = dto.Confirmed,
        SizeId = dto.SizeId,
        Weight = dto.Weight,
        BriefDiscribtion = dto.BriefDiscribtion,
        Discribtion = dto.Discribtion,
      };
      await context.AddAsync<OrderTrackPackage>(record, CancellationToken.None);
      foreach (var item in dto.Source.Where(b => b.Number != 0))
      {
        var itemEntity = new OrderTrackPackageItem
        {
          Package = record,
          PackageId = record.Id,
          Number = item.Number,
          ProductionId = item.ProductionId
        };
        await context.AddAsync<OrderTrackPackageItem>(itemEntity, CancellationToken.None);
      }
      await context.SaveChangesAsync(null);
      return true;
    }
    public async Task<bool> UpdatePackageAsync(PackageDetailDTO dto)
    {
      var record = await context.Query<OrderTrackPackage>(false).Where(b => b.Id == dto.Id).FirstOrDefaultAsync();

      //record.OrderId = dto.OrderId;
      record!.Confirmed = dto.Confirmed;
      record!.SizeId = dto.SizeId;
      record!.Weight = dto.Weight;
      record!.BriefDiscribtion = dto.BriefDiscribtion;
      record!.Discribtion = dto.Discribtion;
      foreach (var item in dto.Source)
      {
        if (item.Id <= 0)
        {
          if (item.Number <= 0)
          {
            continue;
          }
          var itemEntity = new OrderTrackPackageItem
          {
            Package = record,
            PackageId = record.Id,
            Number = item.Number,
            ProductionId = item.ProductionId
          };
          await context.AddAsync<OrderTrackPackageItem>(itemEntity, CancellationToken.None);
        }
        else
        {
          var itemRecord = await context.Query<OrderTrackPackageItem>(false).Where(b => b.Id == item.Id).FirstOrDefaultAsync();
          itemRecord!.PackageId = record.Id;
          itemRecord!.Number = item.Number;
          itemRecord!.ProductionId = item.ProductionId;
        }

      }
      await context.SaveChangesAsync(null);
      return true;
    }
    public async Task<List<SelectedItem>> GetPackageSize()
    {
      var list = new List<SelectedItem>() { new SelectedItem("", "select") };
      var result = await context.Query<OrderTrackPackageSize>(false)
        .Select(b => new SelectedItem { Value = b.Id.ToString(), Text = b.Name })
        .ToListAsync();
      list.AddRange(result);
      return list;
    }
    public async Task<PackageDetailDTO> GetPackageDTO(long id)
    {
      var record = await context.Query<OrderTrackPackage>(false).Where(b => b.Id == id).FirstOrDefaultAsync();
      var order = record!.Order;
      if (record == null)
      {
        return new PackageDetailDTO
        {
          Items = new List<PackageItemDTO>()
        };
      };
      var item = record.Items.Select(b => new PackageItemDTO
      {
        RowId = Guid.NewGuid(),
        Id = b.Id,
        ProductionId = b.ProductionId,
        ProductionName = b.Production.Name,
        Number = b.Number,
      }).ToList();
      var orderItem = order.Items.Select(b => new PackageItemDTO
      {
        RowId = Guid.NewGuid(),
        ProductionId = b.ProductionId,
        ProductionName = b.Production.Name
      }).ToList();
      return new PackageDetailDTO
      {
        OrderId = order.Id,
        RowId = Guid.NewGuid(),
        Id = record.Id,
        Confirmed = record.Confirmed == true,
        SizeId = record.SizeId,
        Weight = record.Weight,
        BriefDiscribtion = record.BriefDiscribtion,
        Discribtion = record.Discribtion,
        Items = item,
        OrderItems = orderItem
      };
    }
    public async Task<PackageDetailDTO> GetNewPackageDTO(long orderId)
    {
      var order = await context.Query<OrderTrackOrder>(false).Where(b => b.Id == orderId).FirstOrDefaultAsync();
      if (order == null)
      {
        return new PackageDetailDTO
        {
          Items = new List<PackageItemDTO>()
        };
      };
      return new PackageDetailDTO
      {
        OrderId = orderId,
        RowId = Guid.NewGuid(),
        OrderItems = order.Items.Select(b => new PackageItemDTO
        {
          RowId = Guid.NewGuid(),
          ProductionId = b.ProductionId,
          ProductionName = b.Production.Name
        }).ToList()
      };
    }

    public async Task<List<PackageDetailDTO>> GetAllPackages(long orderId)
    {
      var order = await context.Query<OrderTrackOrder>(true).Where(b => b.Id == orderId).FirstOrDefaultAsync();
      if (order == null)
      {
        return new List<PackageDetailDTO>();
      };
      return order.Packages.Select(b => new PackageDetailDTO
      {
        Id = b.Id,
        OrderId = orderId,
        BriefDiscribtion = b.BriefDiscribtion,
        Weight = b.Weight,
        Confirmed = b.Confirmed == true,
      }).ToList();
    }
  }
}

