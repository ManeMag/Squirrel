using DataAccess.Contexts;
using DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Squirrel.Extensions;
using Squirrel.Services.Repositories.Abstractions;

namespace Squirrel.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UserRepository(ApplicationContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        public async Task<Result<User>> GetUserWithCategoriesAsync(string id)
        {
            var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Include(u => u.Categories)
                    .FirstOrDefaultAsync();

            if (user is null)
            {
                return Result.Fail("User not found".Using(_localizer));
            }

            return Result.Ok(user);
        }

        public async Task<Result<User>> GetUserWithTransactionsAsync(string id)
        {
            var user = await _context.Users
                    .Where(u => u.Id == id)
                    .Include(u => u.Categories)
                    .ThenInclude(c => c.Transactions)
                    .FirstOrDefaultAsync();

            if (user is null)
            {
                return Result.Fail("User not found".Using(_localizer));
            }

            return Result.Ok(user);
        }
    }
}
