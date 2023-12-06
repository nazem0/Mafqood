using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.IRepositories;
using Domain.Interfaces;

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
