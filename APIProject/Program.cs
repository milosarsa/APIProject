global using Entities.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using APIProject;
using Swashbuckle.AspNetCore.Newtonsoft;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Below section will let us input the token into swagger
builder.Services.AddSwaggerGen(options =>
{
    options.UseOneOfForPolymorphism();
    options.DescribeAllParametersInCamelCase();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APIProject",
        Version = "v1"
    });
    //Below section will let us input the token into swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization using the token generated"
    });


    OpenApiSecurityRequirement keyValuePairs = new OpenApiSecurityRequirement { };
    keyValuePairs.Add(
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    );


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                    }
                });
});
builder.Services.AddAndConfigureScopedDI(builder.Configuration);
builder.Services.AddAndConfigureTransientDI(); 


builder.Services.AddMemoryCache();
builder.Services.AddAndConfigureAuthentication(builder.Configuration); //Need to finish this, read encoding string from app.config
builder.Services.AddAndConfigureAuthorization();//Add policy based authorization

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
