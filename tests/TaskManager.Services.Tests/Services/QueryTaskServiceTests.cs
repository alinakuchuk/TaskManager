using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Models;
using TaskManager.Services.Interfaces;
using TaskManager.Services.MappingProfiles;
using TaskManager.Services.Models;
using TaskManager.Services.Services;
using Xunit;

namespace TaskManager.Services.Tests.Services
{
    public class QueryTaskServiceTests
    {
        private readonly IQueryTaskService _queryTaskService;
        private readonly Mock<ITaskRepository> _mockTaskRepository;

        public QueryTaskServiceTests()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(typeof(DtoTaskMappingProfile)));
            var mapper = new Mapper(configuration);
            _mockTaskRepository = new Mock<ITaskRepository>();
            
            _queryTaskService = new QueryTaskService(
                _mockTaskRepository.Object,
                mapper);
        }

        [Fact]
        public async Task GetTasksAsync_ThereIsFilteringParameter_ReturnTasks()
        {
            var parameters = new DtoGetTasksQueryParameters()
            {
                Limit = 10,
                Offset = 0,
                IsDone = false
            };
            var expectedResult = new List<DbTask>
            {
                new DbTask
                {
                    Id = Guid.NewGuid(),
                    IsDone = false
                },
                new DbTask
                {
                    Id = Guid.NewGuid(),
                    IsDone = false
                }
            };
                
            _mockTaskRepository
                .Setup(repo => repo.GetTasksAsync(
                    It.IsAny<DbGetTasksQueryParameters>(),
                    CancellationToken.None))
                .ReturnsAsync(expectedResult);
            
            var actualResult = await _queryTaskService
                .GetTasksAsync(parameters, CancellationToken.None);
            
            Assert.True(actualResult.FirstOrDefault()!.Id == expectedResult.FirstOrDefault()!.Id);
        }
        
        [Fact]
        public async Task GetTaskByIdAsync_ValidTargetId_ReturnTargetTask()
        {
            var targetId = Guid.NewGuid();
            var expectedResult = new DbTask
            {
                Id = targetId,
                IsDone = false
            };
                
            _mockTaskRepository
                .Setup(repo => repo.GetTaskByIdAsync(
                    It.Is<Guid>(id => id == targetId),
                    CancellationToken.None))
                .ReturnsAsync(expectedResult);
            
            var actualResult = await _queryTaskService
                .GetTaskByIdAsync(targetId, CancellationToken.None);
            
            Assert.True(actualResult.Id == expectedResult.Id);
        }
    }
}