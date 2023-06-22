using AutoMapper;
using TaskManager.Api.Commands;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class DeleteTaskMessageMappingProfile : Profile
    {
        public DeleteTaskMessageMappingProfile()
        {
            CreateMap<DeleteTaskCommand, DeleteTaskMessage>();
        }
    }
}