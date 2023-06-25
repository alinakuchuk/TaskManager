using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public sealed class GetTasksHandlerTests
    {
        private readonly GetTasksHandler _getTasksTaskHandler;
        private readonly Mock<IQueryTaskService> _mockQueryTaskService;

        public GetTasksHandlerTests()
        {
            var configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(typeof(DtoTaskMappingProfile));
                    cfg.AddProfile(typeof(TaskQueryMappingProfile));
                });
            var mapper = new Mapper(configuration);
            _mockQueryTaskService = new Mock<IQueryTaskService>();

            _getTasksTaskHandler = new GetTasksHandler(
                _mockQueryTaskService.Object,
                mapper);
        }

        [Fact]
        public async Task Handle_ValidTargetId_ReturnTargetTask()
        {
            var taskId = Guid.Empty;
            var dueDateTime = new DateTime(2024, 1, 1);
            var task = new GetTasksQuery(dueDateTime, false, 10, 0);
            var expectedResult = new List<DtoTask>
            {
                new DtoTask
                {
                    Id = taskId,
                    DueDateTime = dueDateTime,
                    Name = "Task",
                    Description = "Description"
                }
            };
            _mockQueryTaskService
                .Setup(service => service.GetTasksAsync(
                    It.Is<DtoGetTasksQueryParameters>(param => param.DueDateTime.Value == dueDateTime),
                    CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var actualResult = await _getTasksTaskHandler.Handle(task, CancellationToken.None);

            Assert.True(Guid.Parse(actualResult.FirstOrDefault()!.Id) == expectedResult.FirstOrDefault()!.Id);
        }
    }
}