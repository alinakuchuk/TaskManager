using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManager.Api.Command;
using TaskManager.Api.Commands;
using TaskManager.Api.Handlers;
using TaskManager.Api.MappingProfiles;
using TaskManager.Api.Queries;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;
using Xunit;

namespace TaskManager.Api.Tests.Handlers
{
    public sealed class GetTaskByIdHandlerTests
    {
        private readonly GetTaskByIdHandler _getTaskByIdTaskHandler;
        private readonly Mock<IQueryTaskService> _mockQueryTaskService;

        public GetTaskByIdHandlerTests()
        {
            var configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(typeof(DtoTaskMappingProfile));
                    cfg.AddProfile(typeof(TaskQueryMappingProfile));
                });
            var mapper = new Mapper(configuration);
            _mockQueryTaskService = new Mock<IQueryTaskService>();

            _getTaskByIdTaskHandler = new GetTaskByIdHandler(
                _mockQueryTaskService.Object,
                mapper);
        }

        [Fact]
        public async Task Handle_ValidTargetId_ReturnTargetTask()
        {
            var taskId = Guid.NewGuid();
            var task = new GetTaskByIdQuery(taskId);
            var expectedResult = new DtoTask
            {
                Id = taskId,
                Name = "Task",
                Description = "Description"
            };
            _mockQueryTaskService
                .Setup(service => service.GetTaskByIdAsync(
                    It.Is<Guid>(id => id == taskId),
                    CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var actualResult = await _getTaskByIdTaskHandler.Handle(task, CancellationToken.None);

            Assert.True(Guid.Parse(actualResult.Id) == expectedResult.Id);
        }
    }
}