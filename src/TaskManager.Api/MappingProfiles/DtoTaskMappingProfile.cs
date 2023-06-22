using AutoMapper;
using TaskManager.Api.Query;
using TaskManager.Services.Models;

namespace TaskManager.Api.MappingProfiles
{
    public sealed class DtoTaskMappingProfile : Profile
    {
        public DtoTaskMappingProfile()
        {
            CreateMap<DtoTask, QueryTask>();
        }
    }
}