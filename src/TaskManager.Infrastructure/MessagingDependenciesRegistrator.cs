using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
            
            services.AddScoped<IMessageSender<CreateTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<CreateTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.CreateTaskQueueName);
                return new TaskMessageSender<CreateTaskMessage>(
                    sender,
                    logger,
                    new JsonMessageSerialization<CreateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<UpdateTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<UpdateTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.UpdateTaskQueueName);
                return new TaskMessageSender<UpdateTaskMessage>(
                    sender,
                    logger,
                    new JsonMessageSerialization<UpdateTaskMessage>());
            });
            
            services.AddScoped<IMessageSender<DeleteTaskMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TaskMessageSender<DeleteTaskMessage>>>();
                var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                var sender = serviceBusClient.CreateSender(serviceBusSettings.DeleteTaskQueueName);
                return new TaskMessageSender<DeleteTaskMessage>(
                    sender,
                    logger,
                    new JsonMessageSerialization<DeleteTaskMessage>());
            });

            services.AddSingleton(
                typeof(IMessageSerialization<>),
                typeof(JsonMessageSerialization<>));
        }
    }
}