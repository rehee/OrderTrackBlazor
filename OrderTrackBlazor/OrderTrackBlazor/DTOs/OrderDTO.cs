﻿using BootstrapBlazor.Components;
using Microsoft.IdentityModel.Tokens;

namespace OrderTrackBlazor.DTOs
{
  public class OrderDTO : DTOBase, IWithOrderProductionsDTO
  {
    public DateTime? OrderDate { get; set; }
    public string? ShortNote { get; set; }
    public string? Note { get; set; }
    public List<OrderProductionDTO>? Productions { get; set; }
  }

  public class OrderProductionDTO : DTOBase
  {
    public Guid Gid { get; set; }
    public OrderProductionDTO()
    {
      Gid = Guid.NewGuid();
    }
    public long? ProductionId { get; set; }
    public long? ParentId { get; set; }
    public IWithOrderProductionsDTO? Parent { get; set; }
    public int? Quantity { get; set; }
    public decimal? OrderPrice { get; set; }
    public DateTime? CreateDate { get; set; }

    public long? RecommandShopId { get; set; }
    public string? RecommandShopName { get; set; }
    public long? RecommandShopId2 { get; set; }
    public string? RecommandShopName2 { get; set; }
    public IEnumerable<string?> RecommandShopNames
    {
      get
      {
        return new string?[] { RecommandShopName, RecommandShopName2 };
      }
      set { }
    }
    public string? RecommandShopNameDisplay
    {
      get
      {
        
        return String.Join(",", RecommandShopNames.Where(b => !String.IsNullOrEmpty(b)));
      }
      set { }
    }

    public string? Note { get; set; }
  }

  public interface IWithOrderProductionsDTO
  {
    public long? Id { get; set; }
    List<OrderProductionDTO>? Productions { get; set; }
  }

}
