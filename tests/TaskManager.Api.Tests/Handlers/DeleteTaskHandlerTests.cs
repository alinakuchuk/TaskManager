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
    public sealed class DeleteTaskHandlerTests
    {
        private readonly DeleteTaskHandler _deleteTaskHandler;
        private readonly Mock<IMessageSender<DeleteTaskMessage>> _mockMessageSender;

        public DeleteTaskHandlerTests()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(typeof(TaskCommandMessagingProfile)));
            var mapper = new Mapper(configuration);
            _mockMessageSender = new Mock<IMessageSender<DeleteTaskMessage>>();

            _deleteTaskHandler = new DeleteTaskHandler(
                _mockMessageSender.Object,
                mapper);
        }

        [Fact]
        public async Task Handle_Default_CallSenderSendMethod()
        {
            var taskId = Guid.NewGuid();
            var task = new DeleteTaskCommand(taskId);

            await _deleteTaskHandler.Handle(task, CancellationToken.None);

            _mockMessageSender.Verify(
                sender => sender.SendMessageAsync(
                    It.Is<DeleteTaskMessage>(message => message.Id == taskId)),
                Times.Once);
        }
    }
}