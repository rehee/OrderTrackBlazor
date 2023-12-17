using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using OrderTrackBlazor.Components;
using OrderTrackBlazor.Components.Account;
using OrderTrackBlazor.Data;
using OrderTrackBlazor.Workers;
using ReheeCmf;
using ReheeCmf.Commons.DTOs;
using ReheeCmf.Contexts;
using ReheeCmf.Modules;


namespace OrderTrackBlazor
{
  public class OrderTrackBlazorModule : CmfApiModule<ApplicationDbContext, ApplicationUser>
  {
    public override string ModuleTitle => nameof(OrderTrackBlazorModule);

    public override string ModuleName => nameof(OrderTrackBlazorModule);

    public override Task<IEnumerable<string>> GetPermissions(IContext? db, TokenDTO? user, CancellationToken ct = default)
    {
      return Task.FromResult(Enumerable.Empty<string>());
    }
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.PreConfigureServicesAsync(context);
      context.UseAuthenticationAfterRouting = true;
    }
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
      await base.ConfigureServicesAsync(context);
      context.Services!.AddRazorComponents()
        .AddInteractiveServerComponents();
      context.Services!.AddBootstrapBlazor();
      context.Services!.AddCascadingAuthenticationState();
      context.Services!.AddScoped<IdentityUserAccessor>();
      context.Services!.AddScoped<IdentityRedirectManager>();
      context.Services!.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();


      context.Services!.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
      context.Services!.AddScoped<IOrderService, OrderService>();
      context.Services!.AddScoped<IPurchaseService, PurchaseService>();
      context.Services!.AddScoped<ISummaryService, SummaryService>();
      context.Services!.AddScoped<IStockService, StockService>();
      context.Services!.AddScoped<IDispatchService, DispatchService>();
      context.Services!.AddHostedService<SeedWorker>();
      context.Services!.AddHostedService<EntityCheckWorker>();
      context.Services!.AddScoped<IShopService, ShopService>();
      context.Services!.AddScoped<IPackageService, PackageService>();
    }
    public override async Task BeforePreApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      await base.BeforePreApplicationInitializationAsync(context);
      context.App!.UseHttpsRedirection();
    }
    public override async Task ApplicationInitializationAsync(ServiceConfigurationContext context)
    {
      await base.ApplicationInitializationAsync(context);
      var app = context.App;
      var env = context.Env;

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
        //app.UseReverseProxyHttpsEnforcer();
      }

      context.App!.UseAntiforgery();
      context.App!.MapRazorComponents<App>()
          .AddInteractiveServerRenderMode();

      // Add additional endpoints required by the Identity /Account Razor components.
      context.App!.MapAdditionalIdentityEndpoints();

    }
  }
}
