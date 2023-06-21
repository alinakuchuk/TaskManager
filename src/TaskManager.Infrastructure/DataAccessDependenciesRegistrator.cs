using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManager.DataAccess;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Repositories;
using TaskManager.Infrastructure.Models;

namespace TaskManager.Infrastructure
{
    public static class DataAccessDependenciesRegistrator
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDbSettings"));

            services.AddSingleton(serviceProvider =>
            {
                var cosmosDbSettings = serviceProvider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
                var cosmosClient =  new CosmosClient(cosmosDbSettings.EndpointUri, cosmosDbSettings.PrimaryKey);
                return cosmosClient.GetContainer(cosmosDbSettings.DatabaseName, cosmosDbSettings.ContainerName);
            });
            
            services.AddScoped<ITaskRepository, LocalTaskRepository>();
            services.AddSingleton<IEnumerationBuilder, CosmosEnumerationBuilder>();
        }
    }
}