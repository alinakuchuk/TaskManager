using AutoMapper;
using TaskManager.Api.Query;
using TaskManager.Services.Models;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class QueryTaskMappingProfile : Profile
    {
        public QueryTaskMappingProfile()
        {
            CreateMap<DtoTask, QueryTask>();
        }
    }
}