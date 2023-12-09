using Application.Interfaces.IRepositories;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Mapping;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructureDependencyInjection(this IServiceCollection services)
        {
            services
            .AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}
