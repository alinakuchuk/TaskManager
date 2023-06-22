using System;
using AutoMapper;
using TaskManager.Api.Command;
using TaskManager.Api.Commands;
using TaskManager.Api.Queries;
using TaskManager.Api.Query;

namespace TaskManager.Api.MappingProfiles
{
    public class TaskRequestMappingProfile : Profile
    {
        public TaskRequestMappingProfile()
        {
            CreateMap<CreateTaskRequest, CreateTaskCommand>();
            CreateMap<DeleteTaskRequest, DeleteTaskCommand>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Parse(src.Id)));
            CreateMap<UpdateTaskRequest, UpdateTaskCommand>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Task, act => act.MapFrom(src => src.Task));
            CreateMap<GetTaskRequest, GetTaskByIdQuery>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => Guid.Parse(src.Id)));
        }
    }
}