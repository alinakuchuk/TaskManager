using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using TaskManager.Infrastructure.Models;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.ServiceBus.Services;

namespace TaskManager.Infrastructure
{
    public static class MessagingDependenciesRegistrator
    {
        public static void AddMessagingDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
            
            services.AddSingleton<ServiceBusClient>(provider =>
            {
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                return new ServiceBusClient(serviceBusSettings.ConnectionString);
            });
            
            //TODO: Alina - Log Retry
            services.AddSingleton<AsyncRetryPolicy>(_ =>
            {
                return Policy
                    .Handle<ServiceBusException>()
                    .Or<TimeoutException>()
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, delay, retryAttempt) => throw new Exception($"Retrying due to exception: {exception.Message}. Retry attempt {retryAttempt}."));
            });

            services.AddScoped<IMessagingService<CreateTaskMessage>>(provider =>
            {
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var sender = serviceBusClient.CreateSender(serviceBusSettings.CreateTaskQueueName);
                return new CreateTaskServiceBusMessageService(sender, retryPolicy);
            });
            
            services.AddScoped<IMessagingService<UpdateTaskMessage>>(provider =>
            {
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var sender = serviceBusClient.CreateSender(serviceBusSettings.UpdateTaskQueueName);
                return new UpdateTaskServiceBusMessageService(sender, retryPolicy);
            });
            
            services.AddScoped<IMessagingService<DeleteTaskMessage>>(provider =>
            {
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = provider.GetRequiredService<ServiceBusClient>();
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var sender = serviceBusClient.CreateSender(serviceBusSettings.DeleteTaskQueueName);
                return new DeleteTaskServiceBusMessageService(sender, retryPolicy);
            });
        }
    }
}