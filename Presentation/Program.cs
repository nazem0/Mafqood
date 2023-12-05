using Domain;
using Domain.IRepositories;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

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
                .AddDbContext<EntitiesContext>(dbContext =>
                {
                    #region NazemDB
                    dbContext
                        .UseLazyLoadingProxies()
                        .UseSqlServer
                        (builder.Configuration.GetConnectionString("MyDB"),
                        c => c.EnableRetryOnFailure());
                    #endregion
                    #region SamerDB
                    //dbContext
                    //.UseLazyLoadingProxies()
                    //.UseSqlServer
                    //(builder.Configuration.GetConnectionString("MySamer"),
                    //c => c.EnableRetryOnFailure());
                    #endregion
                })
                .AddHttpContextAccessor()
                .AddControllers();

        services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

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
           .UseAuthorization();

        app.MapControllers();
    }

    private static void RunApplication(WebApplication app)
    {
        app.Run();
    }
}
