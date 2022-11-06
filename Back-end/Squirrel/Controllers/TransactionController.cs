using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Squirrel.Contexts;
using Squirrel.Entities;
using Squirrel.Requests.Transaction;
using Squirrel.Responses.Transaction;

namespace Squirrel.Controllers
{
    public sealed class TransactionController : AuthorizedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public TransactionController(
            IMapper mapper,
            ApplicationContext context,
            UserManager<User> userManager) : base(userManager)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction(
            [FromBody] CreateTransactionRequest transactionRequest)
        {
            var user = _context.Users
                .Include(u => u.Categories)
                .ThenInclude(c => c.Transactions)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            var category = user.Categories
                .Where(c => c.Id == transactionRequest.CategoryId)
                .FirstOrDefault();

            if (category is null)
            {
                return BadRequest("There are not such a category");
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            category.Transactions.Add(transaction);

            return await _context.SaveChangesAsync() > 0
                ? CreatedAtAction(nameof(CreateTransaction), transaction.Id, transaction)
                : BadRequest("Unable to create such a transaction");
        }
    }
}
