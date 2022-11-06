using AutoMapper;
using Squirrel.Entities;
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
            #region Category

            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();

            CreateMap<Category, CategoryViewModel>();

            #endregion

            #region Transaction

            CreateMap<CreateTransactionRequest, Transaction>();

            CreateMap<Transaction, TransactionViewModel>();

            #endregion
        }
    }
}
