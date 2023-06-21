using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using TaskManager.Contracts.Models;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.ServiceBus;

namespace TaskManager.Infrastructure
{
    public static class MessagingDependenciesRegistrator
    {
        public static void AddMessagingDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
            
            services.AddScoped<IMessageSender<CreateTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<CreateTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                return new TaskMessageSender<CreateTaskMessage>(
                    serviceBusClient,
                    serviceBusSettings.CreateTaskQueueName,
                    logger,
                    new JsonMessageSerialization<CreateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<UpdateTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<UpdateTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                return new TaskMessageSender<UpdateTaskMessage>(
                    serviceBusClient,
                    serviceBusSettings.UpdateTaskQueueName,
                    logger,
                    new JsonMessageSerialization<UpdateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<DeleteTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<DeleteTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                return new TaskMessageSender<DeleteTaskMessage>(
                    serviceBusClient,
                    serviceBusSettings.DeleteTaskQueueName,
                    logger,
                    new JsonMessageSerialization<DeleteTaskMessage>());
            });

            services.AddSingleton(
                typeof(IMessageSerialization<>),
                typeof(JsonMessageSerialization<>));
        }
    }
}