using System;
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
    public class CommandTaskServiceTests
    {
        private readonly ICommandTaskService _commandTaskService;
        private readonly Mock<ITaskRepository> _mockTaskRepository;

        public CommandTaskServiceTests()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(typeof(DtoTaskMappingProfile)));
            var mapper = new Mapper(configuration);
            _mockTaskRepository = new Mock<ITaskRepository>();
            
            _commandTaskService = new CommandTaskService(
                _mockTaskRepository.Object,
                mapper);
        }
        
        [Fact]
        public async Task CreateTaskAsync_NewTask_CallRepositoryMethod()
        {
            var task = new DtoTask
            {
                Id = Guid.NewGuid()
            };
            
            await _commandTaskService.CreateTaskAsync(task, CancellationToken.None);
            
            _mockTaskRepository.Verify(
                repository => repository.CreateTaskAsync(
                    It.Is<DbTask>(newTask => newTask.Id == task.Id),
                    CancellationToken.None),
                Times.Once);
        }
        
        [Fact]
        public async Task DeleteTaskAsync_CorrectTaskId_CallRepositoryMethod()
        {
            var taskId = Guid.NewGuid();
            
            await _commandTaskService.DeleteTaskAsync(taskId, CancellationToken.None);
            
            _mockTaskRepository.Verify(
                repository => repository.DeleteTaskAsync(
                    It.Is<Guid>(id => id == taskId),
                    CancellationToken.None),
                Times.Once);
        }
        
        [Fact]
        public async Task UpdateTaskAsync_CorrectTaskId_CallRepositoryMethod()
        {
            var taskId = Guid.NewGuid();
            var task = new DtoTask
            {
                Name = "Updated"
            };
            
            await _commandTaskService.UpdateTaskAsync(taskId, task, CancellationToken.None);
            
            _mockTaskRepository.Verify(
                repository => repository.UpdateTaskAsync(
                    It.Is<Guid>(id => id == taskId),
                    It.Is<DbTask>(updatedTask => updatedTask.Name == task.Name),
                    CancellationToken.None),
                Times.Once);
        }
    }
}