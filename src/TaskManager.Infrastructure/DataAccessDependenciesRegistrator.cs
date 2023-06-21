using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TaskManager.Contracts.Models;
using TaskManager.DataAccess;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Repositories;

namespace TaskManager.Infrastructure
{
    public static class DataAccessDependenciesRegistrator
    {
        public static void AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDbSettings"));

            services.AddSingleton(
                typeof(CosmosClient),
                provider =>
                {
                    var dbOptions = provider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
                    return new CosmosClientBuilder(dbOptions.ConnectionString)
                        .WithSerializerOptions(
                            new CosmosSerializationOptions
                            {
                                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                            }).Build();
                });

            services.AddScoped(
                typeof(Container),
                provider =>
                {
                    var dbOptions = provider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
                    var cosmosClient = provider.GetService<CosmosClient>();

                    return cosmosClient.GetContainer(dbOptions.DatabaseName, dbOptions.ContainerName);
                });
            
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddSingleton<IEnumerationBuilder, CosmosEnumerationBuilder>();
        }
    }
}