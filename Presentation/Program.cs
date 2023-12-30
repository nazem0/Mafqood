using Infrastructure;
using Persistence;
using Serilog;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureLogging(builder);
            ConfigureServices(builder.Services, builder);

            var app = builder.Build();
            ConfigureApp(app);

            app.Run();

        }

        private static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }

        private static void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        {

            services
                .AddCors(option =>
                {
                    option.AddDefaultPolicy(i =>
                    i.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
                })
                .InfrastructureDependencyInjection()
                .PersistenceDependencyInjection()
                .AddDbContext<EntitiesContext>(dbContext =>
                {
                    try
                    {
                        dbContext
                        .UseLazyLoadingProxies()
                        .UseSqlServer
                        (builder.Configuration.GetConnectionString("MyDB"));
                    }
                    catch
                    {
                        dbContext
                        .UseLazyLoadingProxies()
                        .UseSqlServer
                        (builder.Configuration.GetConnectionString("MySamer"));
                    }
                    #region NazemDB
                    //dbContext
                    //    .UseLazyLoadingProxies()
                    //    .UseSqlServer
                    //    (builder.Configuration.GetConnectionString("MyDB"),
                    //    c => c.EnableRetryOnFailure());
                    #endregion
                    #region SamerDB
                    //dbContext
                    //    .UseLazyLoadingProxies()
                    //    .UseSqlServer
                    //    (builder.Configuration.GetConnectionString("MySamer"),
                    //    c => c.EnableRetryOnFailure());
                    #endregion
                })
                .AddHttpContextAccessor()
                .AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EntitiesContext>()
                .AddDefaultTokenProviders();

            services
                    .AddControllers();

            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Mafqood API",
                        Version = "v1"
                    });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer Token\"",
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    Array.Empty<string>()
                    }
                    });
                });
            ;

            services.AddAuthentication(Option =>
            {
                Option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                Option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                };

            });
            // Additional service configurations...
        }

        private static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger().UseSwaggerUI();
            }

            app.UseStaticFiles()
                .UseSerilogRequestLogging()
                .UseHttpsRedirection()
                .UseAuthentication()
                .UseAuthorization();

            app.MapControllers();
        }
    }
}