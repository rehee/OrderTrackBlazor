
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using OrderTrackBlazor.Data;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace OrderTrackBlazor.Components.Pages
{
  public partial class Home
  {
    [Inject]
    [NotNull]
    private DialogService? dialogService { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync();

    }
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
      return base.OnAfterRenderAsync(firstRender);
    }
    public ApplicationUser? Model { get; set; }
    private async Task NormalShowDialog()
    {
      Model = Model ?? new ApplicationUser();
      var items = Utility.GenerateEditorItems<ApplicationUser>();
      var option = new EditDialogOption<ApplicationUser>()
      {
        Title = "edit dialog",
        Model = Model,
        Items = items,
        ItemsPerRow = 2,
        RowType = RowType.Inline,
        OnCloseAsync = () =>
        {
          return Task.CompletedTask;
        },
        OnEditAsync = context =>
        {
          return Task.FromResult(true);
        }
      };

      await dialogService.ShowEditDialog(option);
    }

    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    /// <summary>
    /// get property method
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new()
        {
            Name = "ShowLabel",
            Description = "Whether to show labels",
            Type = "bool",
            ValueList = "true|false",
            DefaultValue = "true"
        },
        new()
        {
            Name = "Model",
            Description = "Generic parameters are used to render the UI",
            Type = "TModel",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new()
        {
            Name = "Items",
            Description = "Edit item collection",
            Type = "IEnumerable<IEditorItem>",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new()
        {
            Name = "DialogBodyTemplate",
            Description = "EditDialog Body template",
            Type = "RenderFragment<TModel>",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new()
        {
            Name = "CloseButtonText",
            Description = "Close button text",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "closure"
        },
        new()
        {
            Name = "SaveButtonText",
            Description = "Save button text",
            Type = "string",
            ValueList = " — ",
            DefaultValue = "save"
        },
        new()
        {
            Name = "OnSaveAsync",
            Description = "Save callback delegate",
            Type = "Func<Task>",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new()
        {
            Name = "ItemsPerRow",
            Description = "Displays the number of components per line",
            Type = "int?",
            ValueList = " — ",
            DefaultValue = " — "
        },
        new()
        {
            Name = "RowType",
            Description = "Set the component layout",
            Type = "RowType",
            ValueList = "Row|Inline",
            DefaultValue = "Row"
        },
        new()
        {
            Name = "LabelAlign",
            Description = "Inline Label alignment in layout mode",
            Type = "Alignment",
            ValueList = "None|Left|Center|Right",
            DefaultValue = "None"
        }
    };
  }
  public class AttributeItem
  {
    /// <summary>
    /// 获得/设置 参数
    /// </summary>
    [DisplayName("参数")]
    public string Name { get; set; } = "";

    /// <summary>
    /// 获得/设置 说明
    /// </summary>
    [DisplayName("说明")]
    public string Description { get; set; } = "";

    /// <summary>
    /// 获得/设置 类型
    /// </summary>
    [DisplayName("类型")]
    public string Type { get; set; } = "";

    /// <summary>
    /// 获得/设置 可选值
    /// </summary>
    [DisplayName("可选值")]
    public string ValueList { get; set; } = "";

    /// <summary>
    /// 获得/设置 默认值
    /// </summary>
    [DisplayName("默认值")]
    public string DefaultValue { get; set; } = "";
  }
}