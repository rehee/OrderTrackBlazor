namespace OrderTrackBlazor.DTOs
{
  public class PackageDetailDTO
  {
    public Guid RowId { get; set; }
    public long? Id { get; set; }
    public long? OrderId { get; set; }
    public long? DispatchId { get; set; }

    public long? PackageId { get; set; }
    
    public int? Number { get; set; }
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
        Number = value;
      }
    }
    public long? SizeId { get; set; }
    public string? SizeName { get; set; }
    public decimal? Weight { get; set; }
    public string? BriefDiscribtion { get; set; }
    public string? BriefDiscribtionFromDispatch{ get; set; }
    public string? Discribtion { get; set; }
    public string? DiscribtionFromDispatch { get; set; }
    public bool Confirmed { get; set; }
    public IEnumerable<PackageItemDTO>? Items { get; set; }
    public IEnumerable<PackageItemDTO>? OrderItems { get; set; }
    public IEnumerable<PackageItemDTO>? Source
    {
      get
      {
        if (Items == null)
        {
          return OrderItems ?? Enumerable.Empty<PackageItemDTO>();
        }
        var orderItems = OrderItems ?? Enumerable.Empty<PackageItemDTO>();
        var ids = Items.Select(b => b.ProductionId).ToArray();
        var result = Items.Concat(orderItems.Where(o => !ids.Contains(o.ProductionId)));
        return result;
      }
    }
  }

  public class PackageItemDTO
  {
    public Guid RowId { get; set; }
    public long Id { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
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
  }
}
