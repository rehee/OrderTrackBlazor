﻿using Microsoft.AspNetCore.Components;
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
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      Console.WriteLine(Context?.GetHashCode());
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
