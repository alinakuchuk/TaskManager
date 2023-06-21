using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Infrastructure.Models;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Interfaces;
using TaskManager.Services.MappingProfiles;
using TaskManager.Services.Services;
using TaskManager.WorkerService.CommandHandlers;

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
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false, false)
                        .AddEnvironmentVariables()
                        .Build();
                    
                    services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));
                    
                    services.AddAutoMapper(typeof(DtoTaskMappingProfile));
                    
                    services.AddScoped<ICommandTaskService, CommandTaskService>();
                    services.AddSingleton(
                        typeof(IMessageSerialization<>),
                        typeof(JsonMessageSerialization<>));

                    services.AddHostedService<CreateTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<CreateTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.CreateTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<CreateTaskMessage>>();
                        var commandTaskService = provider.GetRequiredService<ICommandTaskService>();
                        var mapper = provider.GetRequiredService<IMapper>();
                        return new CreateTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            commandTaskService,
                            mapper);
                    });
                    
                    services.AddHostedService<UpdateTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<UpdateTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.UpdateTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<UpdateTaskMessage>>();
                        var commandTaskService = provider.GetRequiredService<ICommandTaskService>();
                        var mapper = provider.GetRequiredService<IMapper>();
                        return new UpdateTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            commandTaskService,
                            mapper);
                    });
                    
                    services.AddHostedService<DeleteTaskMessageHandler>(provider =>
                    {
                        var serviceBusSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                        var logger = provider.GetRequiredService<ILogger<DeleteTaskMessageHandler>>();
                        var serviceBusClient = new ServiceBusClient(serviceBusSettings.ConnectionString);
                        var receiver = serviceBusClient.CreateReceiver(serviceBusSettings.DeleteTaskQueueName);
                        var messageSerialization = provider.GetRequiredService<IMessageSerialization<DeleteTaskMessage>>();
                        var commandTaskService = provider.GetRequiredService<ICommandTaskService>();
                        return new DeleteTaskMessageHandler(
                            logger,
                            receiver,
                            messageSerialization,
                            commandTaskService);
                    });
                });
    }
}