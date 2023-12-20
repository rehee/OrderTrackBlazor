using System.Drawing;

namespace OrderTrackBlazor.DTOs
{
  public class StockDispatchDTO
  {
    public Guid RowId { get; set; }
    public long Id { get; set; }
    public EnumDispatchStatus? Status { get; set; }
    public DateTime? DispatchDate { get; set; }
    public string DispatchDateFormat
    {
      get
      {
        return DispatchDate?.ToString(Common.DataFormat) ?? "";
      }
      set { }
    }
    public DateTime? CompletedDate { get; set; }
    public decimal? Income { get; set; }
    public string? BriefNote { get; set; }
    public string? Note { get; set; }
    public IEnumerable<StockDispatchPackageDTO>? Packages { get; set; }

    public int PackageNumber
    {
      get
      {
        return Packages?.Sum(b => b.Number) ?? 0;
      }
      set { }
    }
    public decimal CalculateIncome
    {
      get
      {
        return (Income ?? 0) +
          (Packages?.Sum(b => b.Items.Sum(b => b.CalculatePrice)) ?? 0) +
          (Packages?.Sum(b => b.PackagePrice * b.Number) ?? 0);
      }
      set { }
    }
  }

  public class StockDispatchPackageDTO
  {
    public long Id { get; set; }
    public long? PackageSizeId { get; set; }
    public long? StockDispatchId { get; set; }
    public decimal PackagePrice { get; set; }
    public decimal PackageWeight { get; set; }
    public string? BriefDiscribtion { get; set; }
    public string? Discribtion { get; set; }


    public int Number { get; set; }
    public int? NumberInput
    {
      get
      {
        if (Number == 0)
        {
          return null;
        }
        return Number;
      }
      set
      {
        Number = value ?? 0;
      }
    }
    public BootstrapBlazor.Components.Color ColumnColor =>
      items.Any(b => b.InValid) ? BootstrapBlazor.Components.Color.Danger : BootstrapBlazor.Components.Color.Success;
    public decimal PackageIncome => Number * PackagePrice;
    public decimal PackageSpend => OrderItems?.Sum(b => b.CalculatePrice) ?? 0;
    public decimal PackageTotal => PackageIncome + PackageSpend;
    private IEnumerable<StockDispatchPackageItemDTO>? items { get; set; }
    public IEnumerable<StockDispatchPackageItemDTO>? Items
    {
      get
      {
        return items;
      }
      set
      {
        items = value;
        if (value != null)
        {
          foreach (var p in value)
          {
            p.Parent = this;
          }
        }
      }
    }

    public IEnumerable<OrderShortDTO> OrderItems
    {
      get
      {
        var list = new List<OrderShortDTO>();
        if (Items == null)
        {
          return list;
        }
        var result = Items.SelectMany(b => b.OrderItems ?? Enumerable.Empty<OrderShortDTO>())
          .OrderBy(b => b.OrderDate)
          .ThenBy(b => b.ProductionId).ToArray();

        var calculate = result.GroupBy(b => b.ProductionId);
        foreach (var items in calculate)
        {
          var totalItem = Items.FirstOrDefault(b => b.ProductionId == items.Key);
          if (totalItem == null || totalItem.Number * Number == 0)
          {
            foreach (var item in items)
            {

              item.Number = 0;
            }
          }
          else
          {
            var totalNumber = totalItem.Number * Number;
            foreach (var item in items)
            {
              if (totalNumber > item.ShortNumber)
              {
                item.Number = item.ShortNumber;
                totalNumber = totalNumber - item.ShortNumber;
              }
              else
              {
                item.Number = totalNumber;
                totalNumber = 0;
              }
            }
            if (totalNumber > 0)
            {
              var last = items.LastOrDefault();
              if (last != null)
              {
                last.Number = last.Number + totalNumber;
              }
            }
          }
        }

        return result.Where(b => b.Number > 0);
      }
    }

  }
  public class OrderShortDTO
  {
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public long? OrderItemId { get; set; }
    public string? OrderShortNote { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? Price { get; set; }
    public int Number { get; set; }
    public int ShortNumber { get; set; }
    public decimal CalculatePrice
    {
      get
      {
        return Number * (Price ?? 0);
      }
      set { }
    }
  }
  public class StockDispatchPackageItemDTO
  {
    public long Id { get; set; }
    public StockDispatchPackageDTO? Parent { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int AvaliableStock { get; set; }
    public int CalculateStock
    {
      get
      {
        return Parent == null ? 0 : AvaliableStock - Parent.Number * Number;
      }
      set { }
    }
    public IEnumerable<OrderShortDTO>? OrderItems { get; set; }
    public decimal? Price { get; set; }
    public int Number { get; set; }
    public decimal CalculatePrice => (Price ?? 0) * Number;
    public bool InValid => CalculateStock < 0;
    public BootstrapBlazor.Components.Color ColumnColor =>
      InValid ? BootstrapBlazor.Components.Color.Danger :
      CalculateStock == 0 ? BootstrapBlazor.Components.Color.Warning :
      BootstrapBlazor.Components.Color.None;
    public int? NumberInput
    {
      get
      {
        if (Number == 0) return null;
        return Number;
      }
      set
      {
        if (value == null)
        {
          Number = 0;
          return;
        }
        Number = value ?? 0;
      }
    }
    public class StockDispatchRecordItemDTO
    {
      public long? OrderItemId { get; set; }
      public long? ProductionId { get; set; }
      public string? ProductionName { get; set; }
    }
  }
}
