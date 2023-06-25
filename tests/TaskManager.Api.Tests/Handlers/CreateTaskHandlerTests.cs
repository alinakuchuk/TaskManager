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
    public sealed class CreateTaskHandlerTests
    {
        private readonly CreateTaskHandler _createTaskHandler;
        private readonly Mock<IMessageSender<CreateTaskMessage>> _mockMessageSender;

        public CreateTaskHandlerTests()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(typeof(TaskCommandMessagingProfile)));
            var mapper = new Mapper(configuration);
            _mockMessageSender = new Mock<IMessageSender<CreateTaskMessage>>();

            _createTaskHandler = new CreateTaskHandler(
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
            var task = new CreateTaskCommand(commandTask);

            await _createTaskHandler.Handle(task, CancellationToken.None);

            _mockMessageSender.Verify(
                sender => sender.SendMessageAsync(
                    It.Is<CreateTaskMessage>(message => message.Name == commandTask.Name)),
                Times.Once);
        }
    }
}