using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using ReheeCmf.Contexts;
using System.Diagnostics.CodeAnalysis;

namespace OrderTrackBlazor.Components
{
  public class CBase : ComponentBase, IDisposable, IAsyncDisposable
  {
    [Parameter]
    public Func<Task>? ParentOnSave { get; set; }
    [Inject]
    protected IContext? Context { get; set; }
    [Inject]
    protected NavigationManager? nm { get; set; }
    [Inject]
    public DialogService? dialogService { get; set; }
    [Inject]
    [NotNull]
    protected ClipboardService? clipboardService { get; set; }
    public void Dispose()
    {
      DisposeAsync().GetAwaiter().GetResult();
    }
    [Parameter]
    public OnSaveDTO? OnSave { get; set; }
    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();
      if (OnSave != null)
      {
        OnSave.OnSaveFunc = SaveFunction;
      }

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

    public virtual Task<bool> SaveFunction()
    {
      return Task.FromResult(true);
    }
  }
}
