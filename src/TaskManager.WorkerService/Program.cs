using System;
using AutoMapper;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Infrastructure;
using TaskManager.Contracts.Models;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.ServiceBus;
using TaskManager.WorkerService.CommandHandlers;
using TaskManager.WorkerService.MappingProfiles;

namespace TaskManager.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configurationProvider = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false, false)
                        .AddEnvironmentVariables();
                    
                    var keyVaultEndpoint = new Uri(configurationProvider.Build()["KeyVaultSettings:ConnectionString"]!);

                    var configuration = configurationProvider
                        .AddAzureKeyVault(
                            keyVaultEndpoint,
                            new DefaultAzureCredential())
                        .Build();
                    
                    services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));

                    services.AddAutoMapper(typeof(TaskMessageMappingProfile));
                    
                    services.AddDataAccessDependencies(configuration);
                    services.AddServicesDependencies();
                    
                    services.AddSingleton(
                        typeof(IMessageSerialization<>),
                        typeof(JsonMessageSerialization<>));
                    
                    services.AddScoped<IMessageSender<CreateTaskMessage>>(provider =>
                    {
                        var logger = provider.GetRequiredService<ILogger<TaskMessageSender<CreateTaskMessage>>>();
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        return new TaskMessageSender<CreateTaskMessage>(
                            serviceBusClient,
                            serviceBusSettings.ErrorCreateTaskQueueName,
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
                            serviceBusSettings.ErrorUpdateTaskQueueName,
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
                            serviceBusSettings.ErrorDeleteTaskQueueName,
                            logger,
                            new JsonMessageSerialization<DeleteTaskMessage>());
                    });

                    services.AddHostedService<CreateTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<CreateTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.CreateTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<CreateTaskMessage>>();
                        var mapper = provider.GetRequiredService<IMapper>();
                        return new CreateTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            mapper,
                            provider);
                    });
                    
                    services.AddHostedService<UpdateTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<UpdateTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.UpdateTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<UpdateTaskMessage>>();
                        var mapper = provider.GetRequiredService<IMapper>();
                        return new UpdateTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            mapper,
                            provider);
                    });
                    
                    services.AddHostedService<DeleteTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<DeleteTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.DeleteTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<DeleteTaskMessage>>();
                        return new DeleteTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            provider);
                    });
                });
    }
}