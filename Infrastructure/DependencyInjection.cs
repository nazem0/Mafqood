using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IReportRepository, ReportRepository>();
            return services;
        }
    }
}
