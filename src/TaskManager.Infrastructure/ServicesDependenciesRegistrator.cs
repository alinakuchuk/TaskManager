using Microsoft.Extensions.DependencyInjection;
using TaskManager.Services.Interfaces;
using TaskManager.Services.MappingProfiles;
using TaskManager.Services.Services;

namespace TaskManager.Infrastructure
{
    public static class ServicesDependenciesRegistrator
    {
        public static void AddServicesDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DtoTaskMappingProfile));
            
            services.AddScoped<IQueryTaskService, QueryTaskService>();
        }
    }
}