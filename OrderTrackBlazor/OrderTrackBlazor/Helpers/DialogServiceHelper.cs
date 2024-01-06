using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using OrderTrackBlazor.Components;
using OrderTrackBlazor.Components.Pages.EntityComponents;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace OrderTrackBlazor.Helpers
{
  public static class DialogServiceHelper
  {
    public static async Task ShowComponent<T>(this DialogService? dialogService, Dictionary<string, object?>? parameter = null,
      string title = "", bool showSave = false, Func<OnSaveDTO, Task>? onSaveSuccess = null, Size size = Size.ExtraLarge
      ) where T : CBase
    {
      var onSave = new OnSaveDTO();
      var parameterInput = parameter ?? new Dictionary<string, object?>();
      if (!parameterInput.ContainsKey("OnSave"))
      {
        parameterInput["OnSave"] = onSave;
      }

      var comp = BootstrapDynamicComponent.CreateComponent<T>(parameterInput);
      var saveOnProgress = false;
      var dotion = new DialogOption()
      {
        IsScrolling = true,
        Title = title,
        Size = size,
        Component = comp,
        ShowSaveButton = showSave,
        OnSaveAsync = async () =>
        {
          await Task.CompletedTask;
          if (saveOnProgress)
          {
            return false;
          }
          saveOnProgress = true;
          if (onSave.OnSaveFunc != null)
          {
            var result = await onSave.OnSaveFunc();
            if (result)
            {
              if (onSaveSuccess != null)
              {
                await onSaveSuccess(onSave);
              }
            }
            saveOnProgress = false;
            return result;
          }
          else
          {
            saveOnProgress = false;
            return true;
          }
        }

      };
      await dialogService!.Show(dotion);
    }
  }
}
