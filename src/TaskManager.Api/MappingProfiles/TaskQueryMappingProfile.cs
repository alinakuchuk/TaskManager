using AutoMapper;
using TaskManager.Api.Queries;
using TaskManager.Services.Models;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class TaskQueryMappingProfile : Profile
    {
        public TaskQueryMappingProfile()
        {
            CreateMap<GetTasksQuery, DtoGetTasksQueryParameters>();
        }
    }
}