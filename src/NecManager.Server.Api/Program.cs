using Microsoft.AspNetCore.Identity;

using NecManager.Server.Api.Business.TokenProviders;
using NecManager.Server.Api.Modules;
using NecManager.Server.Api.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.RegisterModules<Program>();

builder.Services.AddSecurityDefinition(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "NecManager.Server.Api", Version = "v1" }));

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(value: 7));

builder.Services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(value: 7));


var app = builder.Build();

app.UseModulesBuildActions();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NecManager.Server.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapModulesEndpoints();

app.Run();
