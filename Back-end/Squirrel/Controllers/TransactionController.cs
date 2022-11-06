using AutoMapper;
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
            var category = GetCategory(transactionRequest.CategoryId);

            if (category is null)
            {
                return BadRequest("There are not such a category");
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            category.Transactions.Add(transaction);
            var added = await _context.SaveChangesAsync() > 0;

            if (!added)
            {
                BadRequest("Unable to create such a transaction");
            }

            var transactionResponse = _mapper.Map<TransactionViewModel>(transaction);

            return CreatedAtAction(nameof(CreateTransaction), transactionResponse.Id, transactionResponse);
        }

        [HttpPatch]
        public async Task<ActionResult<TransactionViewModel>> UpdateTransaction(
            [FromBody] UpdateTransactionRequest transactionRequest)
        {
            var category = GetCategory(transactionRequest.CategoryId);

            if (category is null)
            {
                return BadRequest("There are not such a category");
            }

            var transaction = GetTransaction(transactionRequest.Id);

            if (transaction is null)
            {
                return BadRequest("There are not such a transaction");
            }

            _mapper.Map(transactionRequest, transaction);

            _context.Transactions.Update(transaction);

            return await _context.SaveChangesAsync() > 0
                ? Ok(_mapper.Map<TransactionViewModel>(transaction))
                : BadRequest("Unable to update this transaction");
        }

        private Category GetCategory(int id)
        {
            var user = _context.Users
                .Include(u => u.Categories)
                .ThenInclude(c => c.Transactions)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            var category = user.Categories
                .Where(c => c.Id == id)
                .FirstOrDefault();

            return category;
        }

        private Transaction GetTransaction(int id) =>
            _context.Transactions.Where(t => t.Id == id).FirstOrDefault();
    }
}
