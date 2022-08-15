using BlackBlueBeesStudio.Tools.Component.Blazor.Notifications.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NecManager.Web;

var builder = WebApplication.CreateBuilder(args);

// Configures WebHost
builder.WebHost.UseIISIntegration();

ServiceCollection(builder.Services);

var application = builder.Build();

ServicePipeline(application);

/// <summary>
///     Method which configure service for DI.
/// </summary>
/// <param name="services">The services collection to inject.</param>
void ServiceCollection(IServiceCollection services)
{
    // TODO builder.Host.UseCommonLogger();
    services.AddWebServices();

    // Add services to the container.
    services.AddRazorPages();
    services.AddServerSideBlazor();

    // Add Blazor component
    services.AddToastManagement();

}
void ServicePipeline(WebApplication? app)
{

    // Configure the HTTP request pipeline.
    if (app?.Environment.IsDevelopment() ?? false)
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app?.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app?.UseHsts();
    }

    app?.UseHttpsRedirection();

    app?.UseStaticFiles();

    app?.UseRouting();


    app?.MapBlazorHub();
    app?.MapFallbackToPage("/_Host");

    app?.Run();
}
