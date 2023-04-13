
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NecManager.Web;
using NecManager.Web.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NecManagerWebContextConnection") ?? throw new InvalidOperationException("Connection string 'NecManagerWebContextConnection' not found.");

builder.Services.AddDbContext<NecManagerWebContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<NecManagerWebUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<NecManagerWebContext>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

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
