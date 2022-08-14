using NecManager.Server.Api.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.RegisterModules<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "NecManager.Server.Api", Version = "v1" }));


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

app.MapModulesEndpoints();

app.Run();
