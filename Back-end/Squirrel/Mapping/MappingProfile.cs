using AutoMapper;
using Squirrel.Entities;
using Squirrel.Requests.Category;

namespace Squirrel.Mapping
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCategoryRequest, Category>();
        }
    }
}
