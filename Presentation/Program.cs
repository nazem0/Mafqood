using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Domain;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
        builder.Services.AddAutoMapper(typeof(MappingProfiles));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IReportRepository, ReportRepository>();
        builder.Services.AddDbContext<EntitiesContext>(dbContext =>
        {
            dbContext
            .UseLazyLoadingProxies()
            .UseSqlServer
            (builder.Configuration.GetConnectionString("MyDB"),
            c => c.EnableRetryOnFailure());
            #region SamerDB
            //dbContext
            //.UseLazyLoadingProxies()
            //.UseSqlServer
            //(builder.Configuration.GetConnectionString("MySamer"),
            //c => c.EnableRetryOnFailure());
            #endregion
        });
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        var supportedCultures = new[] { new CultureInfo("ar-EG") }; // for Arabic
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("ar-EG"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
        app.UseStaticFiles();
        app.UseSerilogRequestLogging();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}