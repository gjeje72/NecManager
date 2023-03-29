using BlackBlueBeesStudio.Tools.Component.Blazor.Notifications.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NecManager.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NecManager.Web.Data;
using NecManager.Web.Areas.Identity.Data;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NecManagerWebContextConnection") ?? throw new InvalidOperationException("Connection string 'NecManagerWebContextConnection' not found.");

builder.Services.AddDbContext<NecManagerWebContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<NecManagerWebUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<NecManagerWebContext>();

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
