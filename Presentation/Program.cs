using Infrastructure;
using Persistence;
using Serilog;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureLogging(builder);
        ConfigureServices(builder.Services, builder);

        var app = builder.Build();
        ConfigureApp(app);
        RunApplication(app);
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
        services.InfrastructureDependencyInjection()
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
                .AddControllers();

        services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

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
        var supportedCultures = new[] { new CultureInfo("ar-EG") }; // for Arabic
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("ar-EG"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseStaticFiles()
           .UseSerilogRequestLogging()
           .UseSwagger()
           .UseSwaggerUI()
           .UseHttpsRedirection()
           .UseAuthentication()
           .UseAuthorization();
        app.MapControllers();
    }

    private static void RunApplication(WebApplication app)
    {
        app.Run();
    }
}
