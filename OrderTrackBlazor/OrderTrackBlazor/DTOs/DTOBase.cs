namespace OrderTrackBlazor.DTOs
{
  public abstract class DTOBase
  {
    public virtual long? Id { get; set; }
    public bool IsCreate => Id.HasValue != true || Id <= 0;
  }
}
