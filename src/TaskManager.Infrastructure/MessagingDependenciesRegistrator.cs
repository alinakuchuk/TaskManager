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
using TaskManager.ServiceBus;

namespace TaskManager.Infrastructure
{
    public static class MessagingDependenciesRegistrator
    {
        public static void AddMessagingDependencies(this IServiceCollection services, IConfiguration configuration)
        {
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

            services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
            
            services.AddScoped<IMessageSender<CreateTaskMessage>>(provider =>
            {
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.CreateTaskQueueName);
                return new TaskMessageSender<CreateTaskMessage>(
                    sender,
                    retryPolicy,
                    new JsonMessageSerialization<CreateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<UpdateTaskMessage>>(provider =>
            {
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.UpdateTaskQueueName);
                return new TaskMessageSender<UpdateTaskMessage>(
                    sender,
                    retryPolicy,
                    new JsonMessageSerialization<UpdateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<DeleteTaskMessage>>(provider =>
            {
                var retryPolicy = provider.GetRequiredService<AsyncRetryPolicy>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.DeleteTaskQueueName);
                return new TaskMessageSender<DeleteTaskMessage>(
                    sender,
                    retryPolicy,
                    new JsonMessageSerialization<DeleteTaskMessage>());
            });

            services.AddSingleton(
                typeof(IMessageSerialization<>),
                typeof(JsonMessageSerialization<>));
        }
    }
}