namespace OrderTrackBlazor.DTOs
{
  public class DispatchDetailDTO
  {
    public long? Id { get; set; }
    public long? OrderId { get; set; }
    public DateTime? DispatchDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? IncomeDate { get; set; }
    public EnumDispatchStatus? Status { get; set; }
    public decimal? Income { get; set; }
    public decimal TotalIncome
    {
      get
      {
        var sum = EditItems.Sum(b => b.TotalPrice);
        var income = Income.HasValue ? Income.Value : 0;
        return income + sum;
      }
      set
      {
      }
    }
    public string? Note { get; set; }
    public decimal? PackageNumber { get; set; }
    public IEnumerable<DispatchDetailItemDTO>? Items { get; set; }
    public IEnumerable<DispatchDetailItemDTO>? OrderItems { get; set; }

    public IEnumerable<DispatchDetailItemDTO> EditItems
    {
      get
      {
        if (Items == null)
        {
          Items = new List<DispatchDetailItemDTO>();
        }
        if (OrderItems == null)
        {
          return Items;
        }
        var pIds = Items.Select(x => x.ProductionId).ToArray();
        return Items.Concat(OrderItems.Where(o => pIds.Contains(o.ProductionId) != true));
      }
    }
  }

  public class DispatchDetailItemDTO
  {

    public Guid RowId { get; set; }
    public long? Id { get; set; }
    public long? OrderProductionId { get; set; }
    public long? ProductionId { get; set; }
    public string? ProductionName { get; set; }
    public int Number { get; set; }
    public string? NumberInput
    {
      get
      {
        if (Number == 0)
        {
          return null;
        }
        return Number.ToString();
      }
      set
      {
        if (int.TryParse(value, out var v))
        {
          Number = v;
          
        }
        else
        {
          Number = 0;
        }
      }
    }
    public decimal? DispatchPrice { get; set; }
    public decimal TotalPrice
    {
      get
      {
        if (DispatchPrice.HasValue != true)
        {
          return 0;
        }
        return Number * DispatchPrice.Value;
      }
      set
      {

      }
    }
  }
}
