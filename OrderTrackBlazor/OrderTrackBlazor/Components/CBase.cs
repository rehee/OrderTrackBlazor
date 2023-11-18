using Microsoft.AspNetCore.Components;
using ReheeCmf.Contexts;

namespace OrderTrackBlazor.Components
{
  public class CBase : ComponentBase, IDisposable, IAsyncDisposable
  {
    [Inject]
    protected IContext? Context { get; set; }
    public void Dispose()
    {
      DisposeAsync().GetAwaiter().GetResult();
    }

    public bool IsDisposed { get; protected set; }
    public virtual ValueTask DisposeAsync()
    {
      if (IsDisposed)
      {
        return ValueTask.CompletedTask;
      }
      IsDisposed = true;
      //if (Context != null)
      //{
      //  Context.Dispose();
      //}
      return ValueTask.CompletedTask;
    }
  }
}
