using AutoMapper;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Models;

namespace TaskManager.WorkerService.MappingProfiles
{
    public sealed class TaskMessageMappingProfile : Profile
    {
        public TaskMessageMappingProfile()
        {
            CreateMap<CreateTaskMessage, DtoTask>();
            CreateMap<UpdateTaskMessage, DtoTask>();
        }
    }
}