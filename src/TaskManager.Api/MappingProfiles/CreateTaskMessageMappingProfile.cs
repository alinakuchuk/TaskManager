using System;
using AutoMapper;
using TaskManager.Api.Commands;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class CreateTaskMessageMappingProfile : Profile
    {
        public CreateTaskMessageMappingProfile()
        {
            CreateMap<CreateTaskCommand, CreateTaskMessage>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Task.Name))
                .ForMember(dest => dest.IsDone, act => act.MapFrom(src => src.Task.IsDone))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src.Task.Description))
                .ForMember(dest => dest.DueDateTime, act => act.MapFrom(src => DateTime.Parse(src.Task.DueDateTime)));
        }
    }
}