using Interfaces.Repo;
using Interfaces.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repo;
using Service;
using System.Text;

namespace APIProject
{
    public static class ServiceCollectionExtensions
    {
        //Setting up JWT Bearer Authentication
        public static IServiceCollection AddAndConfigureAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["EncodingString"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(30)
                };
            });

            return service;
        }
        //We are using a simple policy based authorization to better manage who has access to what
        public static IServiceCollection AddAndConfigureAuthorization(this IServiceCollection service)
        {
            service.AddAuthorization(options =>
            {
                options.AddPolicy("User",
                    policy =>
                    {
                        policy.RequireRole("User", "Manager", "Admin");
                    });
                options.AddPolicy("Manager",
                    policy =>
                    {
                        policy.RequireRole("Manager", "Admin");
                    });
                options.AddPolicy("Admin",
                    policy =>
                    {
                        policy.RequireRole("Admin");
                    });
            });

            return service;
        }
        public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(options =>
            {
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

            return service;
        }

        public static IServiceCollection AddAndConfigureScopedDI(this IServiceCollection service, IConfiguration configuration)
        {

            var _storageConnectionString = configuration.GetConnectionString("StorageConnectionString");
            service.AddScoped(storageAccount => CloudStorageAccount.Parse(_storageConnectionString));

            return service;
        }

        public static IServiceCollection AddAndConfigureTransientDI(this IServiceCollection service)
        {
            service.AddTransient<IProjectService, ProjectService>();
            service.AddTransient<IProjectRepo, ProjectRepo>();
            service.AddTransient<ITaskService, TaskService>();
            service.AddTransient<ITaskRepo, TaskRepo>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IUserRepo, UserRepo>();
            return service;
        }
    }
}
