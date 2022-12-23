using AutoMapper;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Squirrel.Constants.Wording;
using Squirrel.Extensions;
using Squirrel.Requests.Category;
using Squirrel.Responses.Category;
using Squirrel.Services;

namespace Squirrel.Controllers
{
    using static Wording.Category;

    public sealed class CategoryController : AuthorizedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly BaseCategorySeeder _seeder;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoryController(
            IMapper mapper,
            ApplicationContext context,
            BaseCategorySeeder seeder,
            IStringLocalizer<SharedResource> localizer,
            UserManager<User> userManager) : base(userManager)
        {
            _mapper = mapper;
            _context = context;
            _seeder = seeder;
            _localizer = localizer;
        }

        [HttpGet("{type}")]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories(int type)
        {
            var user = _context.Users
                .Include(u => u.Categories)!
                .ThenInclude(c => c.Transactions)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            if (user is null)
            {
                return BadRequest();
            }

            if (!user.Categories!.Any())
            {
                var seedingResult = await _seeder.SeedCategories(user.Id);

                if (!seedingResult.IsSuccess)
                {
                    return BadRequest(seedingResult.Errors);
                }

                user = _context.Users
                    .Include(u => u.Categories)!
                    .ThenInclude(c => c.Transactions)
                    .Where(u => u.Id == GetUserId())
                    .FirstOrDefault();
            }

            return Ok(_mapper.Map<IEnumerable<CategoryViewModel>>(user!.Categories?.Where(c => c.Type == (DataAccess.Entities.Type)type || c.Type == 0)));
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromForm] CreateCategoryRequest categoryRequest)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryRequest);

                var user = _context.Users
                    .Include(u => u.Categories)
                    .Where(u => u.Id == GetUserId())
                    .FirstOrDefault();

                if (user is null)
                {
                    return BadRequest();
                }

                user.Categories!.Add(category);
                var added = await _context.SaveChangesAsync() > 0;

                if (!added)
                {
                    return BadRequest(CannotCreate.Using(_localizer));
                }

                var categoryResponse = _mapper.Map<CategoryViewModel>(category);

                return CreatedAtAction(nameof(CreateCategory), categoryResponse.Id, categoryResponse);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException && (ex.InnerException as SqliteException)!.SqliteErrorCode == 19)
                    return BadRequest("This category already exists".Using(_localizer));
            }
            return BadRequest(_localizer["Somthing went wrong"].Value);
        }

        [HttpPatch]
        public async Task<ActionResult<CategoryViewModel>> UpdateCategory([FromForm] UpdateCategoryRequest categoryRequest)
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.Categories)
                    .Where(u => u.Id == GetUserId())
                    .FirstOrDefault();

                if (user is null)
                {
                    return BadRequest();
                }

                var category = user.Categories!
                    .Where(c => c.Id == categoryRequest.Id)
                    .FirstOrDefault();

                if (category is null)
                    return NotFound();

                _mapper.Map<UpdateCategoryRequest, Category>(categoryRequest, category);

                return await _context.SaveChangesAsync() > 0
                    ? Ok(_mapper.Map<CategoryViewModel>(category))
                    : NoContent();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqliteException && (ex.InnerException as SqliteException)!.SqliteErrorCode == 19)
                    return BadRequest("This category already exists".Using(_localizer));
            }
            return BadRequest(_localizer["Somthing went wrong"].Value);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryViewModel>> RemoveCategory(int id)
        {
            var user = _context.Users
                .Include(u => u.Categories)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            if (user is null)
            {
                return BadRequest();
            }

            var category = user.Categories!
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (category is null)
            {
                return NotFound();
            }

            if (category!.IsBaseCategory)
            {
                return BadRequest(DeletingBaseCategory.Using(_localizer));
            }

            user.Categories!.Remove(category);

            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : NoContent();
        }
    }
}
