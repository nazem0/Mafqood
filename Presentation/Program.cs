using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAutoMapper(typeof(MappingProfiles));
        builder.Services.AddControllers();
        builder.Services.AddDbContext<EntitiesContext>(context =>
        {
            context
            .UseLazyLoadingProxies()
            .UseSqlServer
            (builder.Configuration.GetConnectionString("MyDB"),
            c => c.EnableRetryOnFailure());
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        app.UseStaticFiles();
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