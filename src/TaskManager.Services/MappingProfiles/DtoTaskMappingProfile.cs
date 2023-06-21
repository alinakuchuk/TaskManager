using AutoMapper;
using TaskManager.DataAccess.Models;
using TaskManager.Services.Models;

namespace TaskManager.Services.MappingProfiles
{
    public sealed class DtoTaskMappingProfile : Profile
    {
        public DtoTaskMappingProfile()
        {
            CreateMap<DtoGetTasksQueryParameters, DbGetTasksQueryParameters>();
            CreateMap<DtoTask, DbTask>();
        }
    }
}