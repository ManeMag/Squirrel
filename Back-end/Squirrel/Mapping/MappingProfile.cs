using AutoMapper;
using Squirrel.Entities;
using Squirrel.Requests.Category;
using Squirrel.Responses.Category;

namespace Squirrel.Mapping
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category

            CreateMap<CreateCategoryRequest, Category>();

            CreateMap<Category, CategoryViewModel>();

            #endregion
        }
    }
}
