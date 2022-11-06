using AutoMapper;
using DataAccess.Entities;
using Squirrel.Models;
using Squirrel.Requests.Category;
using Squirrel.Requests.Transaction;
using Squirrel.Responses.Category;
using Squirrel.Responses.Transaction;

namespace Squirrel.Mapping
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User

            CreateMap<RegisterModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(src => src.Email));

            #endregion

            #region Category

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            CreateMap<Category, CategoryViewModel>();

            #endregion

            #region Transaction

            CreateMap<CreateTransactionRequest, Transaction>();
            CreateMap<UpdateTransactionRequest, Transaction>();

            CreateMap<Transaction, TransactionViewModel>();

            #endregion
        }
    }
}
