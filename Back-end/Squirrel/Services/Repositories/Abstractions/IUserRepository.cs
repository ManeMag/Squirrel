using DataAccess.Entities;
using FluentResults;

namespace Squirrel.Services.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<Result<User>> GetUserWithCategoriesAsync(string id);
    }
}
