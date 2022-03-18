global using Entities.Enums;
using APIProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAndConfigureSwagger();
builder.Services.AddAndConfigureScopedDI(builder.Configuration);
builder.Services.AddAndConfigureTransientDI();
builder.Services.AddMemoryCache();
builder.Services.AddAndConfigureAuthentication(builder.Configuration); //Need to finish this, read encoding string from app.config
builder.Services.AddAndConfigureAuthorization();//Add policy based authorization
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
