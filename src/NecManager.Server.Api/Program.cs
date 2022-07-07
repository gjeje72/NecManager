using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ServiceCollection(builder.Services);

var application = builder.Build();

ServicePipeline(application);

void ServiceCollection(IServiceCollection services)
{

    // Add services to the container.

    services.AddControllers();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "NecManager.Server.Api", Version = "v1" });
    });
}

void ServicePipeline(WebApplication? app)
{
    // Configure the HTTP request pipeline.
    if (builder.Environment.IsDevelopment())
    {
        app?.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NecManager.Server.Api v1"));
    }

    app?.UseHttpsRedirection();

    app?.UseAuthorization();

    app?.MapControllers();

    app?.Run();
}
