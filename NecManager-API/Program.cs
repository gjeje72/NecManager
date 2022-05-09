using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NecManager_API.Modules;
using NecManager_DAL.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterModules();

builder.Services.AddData(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")), builder.Environment);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NecManager.Server.Api", Version = "v1" });
});

var app = builder.Build();
app.UseModuleBuildActions();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NecManager.Server.Api v1");
        options.DisplayOperationId();
    });
}

app.UseHttpsRedirection();

app.Run();