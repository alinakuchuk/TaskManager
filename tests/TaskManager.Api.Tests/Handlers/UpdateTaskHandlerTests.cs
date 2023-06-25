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
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using Xunit;

namespace TaskManager.Api.Tests.Handlers
{
    public sealed class UpdateTaskHandlerTests
    {
        private readonly UpdateTaskHandler _updateTaskHandler;
        private readonly Mock<IMessageSender<UpdateTaskMessage>> _mockMessageSender;

        public UpdateTaskHandlerTests()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(typeof(TaskCommandMessagingProfile)));
            var mapper = new Mapper(configuration);
            _mockMessageSender = new Mock<IMessageSender<UpdateTaskMessage>>();

            _updateTaskHandler = new UpdateTaskHandler(
                _mockMessageSender.Object,
                mapper);
        }

        [Fact]
        public async Task Handle_Default_CallSenderSendMethod()
        {
            var commandTask = new CommandTask
            {
                Name = "Task",
                DueDateTime = new DateTime(2024, 1, 1).ToString(CultureInfo.InvariantCulture)
            };
            var taskId = Guid.NewGuid();
            var task = new UpdateTaskCommand(taskId, commandTask);

            await _updateTaskHandler.Handle(task, CancellationToken.None);

            _mockMessageSender.Verify(
                sender => sender.SendMessageAsync(
                    It.Is<UpdateTaskMessage>(message => message.Name == commandTask.Name && message.Id == taskId)),
                Times.Once);
        }
    }
}