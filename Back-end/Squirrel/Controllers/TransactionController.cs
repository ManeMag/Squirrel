using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Squirrel.Constants.Wording;
using Squirrel.Contexts;
using Squirrel.Entities;
using Squirrel.Extensions;
using Squirrel.Requests.Transaction;
using Squirrel.Responses.Transaction;

namespace Squirrel.Controllers
{
    using static Wording.Transaction;
    using static Wording.Category;
    public sealed class TransactionController : AuthorizedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly IStringLocalizer _localizer;

        public TransactionController(
            IMapper mapper,
            ApplicationContext context,
            IStringLocalizer localizer,
            UserManager<User> userManager) : base(userManager)
        {
            _mapper = mapper;
            _context = context;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionViewModel>>> GetTransactions()
        {
            var user = GetUser();

            var transactions = user.Categories.SelectMany(c => c.Transactions);

            return Ok(_mapper.Map<IEnumerable<TransactionViewModel>>(transactions));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransactionViewModel>> GetTransactionById(int id)
        {
            var transaction = GetTransaction(id);

            if (!HasTransaction(transaction))
            {
                return BadRequest(HasNoTransaction.Using(_localizer));
            }

            return _mapper.Map<TransactionViewModel>(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionViewModel>> CreateTransaction(
            [FromBody] CreateTransactionRequest transactionRequest)
        {
            var category = GetCategory(transactionRequest.CategoryId);

            if (category is null)
            {
                return BadRequest(NoSuchCategory.Using(_localizer));
            }

            var transaction = _mapper.Map<Transaction>(transactionRequest);

            category.Transactions.Add(transaction);
            var added = await _context.SaveChangesAsync() > 0;

            if (!added)
            {
                BadRequest(UnableToCreate.Using(_localizer));
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
                return BadRequest(NoSuchCategory.Using(_localizer));
            }

            var transaction = GetTransaction(transactionRequest.Id);

            if (transaction is null)
            {
                return BadRequest(HasNoTransaction.Using(_localizer));
            }

            _mapper.Map(transactionRequest, transaction);

            _context.Transactions.Update(transaction);

            return await _context.SaveChangesAsync() > 0
                ? Ok(_mapper.Map<TransactionViewModel>(transaction))
                : BadRequest(UnableToUpdate.Using(_localizer));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTransaction(int id)
        {
            var transaction = GetTransaction(id);

            if (!HasTransaction(transaction))
            {
                return BadRequest(HasNoTransaction.Using(_localizer));
            }

            _context.Transactions.Remove(transaction);

            var removed = await _context.SaveChangesAsync() > 0;

            return removed ? Ok() : NoContent();
        }

        private bool HasTransaction(Transaction transaction)
        {
            if (transaction is null)
            {
                return false;
            }

            var category = GetCategory(transaction.CategoryId);
            var user = GetUser();

            var userHasCategoryOfTransaction = user.Categories
                .Where(c => c.Id == category.Id)
                .Any();

            return userHasCategoryOfTransaction;
        }

        private Category GetCategory(int id)
        {
            var user = GetUser();

            var category = user.Categories
                .Where(c => c.Id == id)
                .FirstOrDefault();

            return category;
        }

        private User GetUser()
        {
            return _context.Users
                .Include(u => u.Categories)
                .ThenInclude(c => c.Transactions)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();
        }

        private Transaction GetTransaction(int id) =>
            _context.Transactions.Where(t => t.Id == id).FirstOrDefault();
    }
}
