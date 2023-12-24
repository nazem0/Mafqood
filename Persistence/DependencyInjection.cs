using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Application.Interfaces;
using Application.Interfaces.IRepositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection PersistenceDependencyInjection(this IServiceCollection services)
        {
            services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IReportRepository, ReportRepository>()
            .AddScoped<IAccountRepository, AccountRepository>();
            return services;
        }
    }
}
