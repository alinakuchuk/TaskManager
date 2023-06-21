using AutoMapper;
using TaskManager.Api.Queries;
using TaskManager.Services.Models;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class GetTasksQueryMappingProfile : Profile
    {
        public GetTasksQueryMappingProfile()
        {
            CreateMap<GetTasksQuery, DtoGetTasksQueryParameters>();
        }
    }
}