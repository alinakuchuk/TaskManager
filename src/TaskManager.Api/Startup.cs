using System.Reflection;
using Calzolari.Grpc.AspNetCore.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManager.Api.Services;
using TaskManager.Api.Validators;
using TaskManager.Infrastructure;

namespace TaskManager.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole(); 
            });
            
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.Interceptors.Add<ExceptionInterceptor>();
            });
            
            services.AddGrpcValidation();

            services.AddValidator<CreateTaskRequestValidator>();
            services.AddValidator<UpdateTaskRequestValidator>();
            services.AddValidator<DeleteTaskRequestValidator>();
            services.AddValidator<GetTaskRequestValidator>();
            services.AddValidator<GetTasksRequestValidator>();

            services.AddGrpcReflection();
            
            services.AddDataAccessDependencies(_configuration);
            services.AddMessagingDependencies(_configuration);
            services.AddServicesDependencies();
            
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(cfg 
                => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcReflectionService();
                endpoints.MapGrpcService<QueryManagedTaskService>();
                endpoints.MapGrpcService<CommandManagedTaskService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}