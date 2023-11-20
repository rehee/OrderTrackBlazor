namespace OrderTrackBlazor.Services
{
  public interface ISummaryService
  {
    IQueryable<SummaryDTO> Query();
  }
}
