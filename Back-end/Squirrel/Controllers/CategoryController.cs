using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Squirrel.Contexts;
using Squirrel.Entities;
using Squirrel.Requests.Category;
using Squirrel.Responses.Category;
using Squirrel.Services;

namespace Squirrel.Controllers
{
    public sealed class CategoryController : AuthorizedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly BaseCategoriesSeeder _seeder;

        public CategoryController(
            IMapper mapper,
            ApplicationContext context,
            BaseCategoriesSeeder seeder,
            UserManager<User> userManager) : base(userManager)
        {
            _mapper = mapper;
            _context = context;
            _seeder = seeder;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            var user = _context.Users
                .Include(u => u.Categories)
                .ThenInclude(c => c.Transactions)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            if (user is null)
            {
                return BadRequest();
            }

            if (!user.Categories.Any())
            {
                var seedingResult = await _seeder.SeedCategories(user.Id);

                if (!seedingResult.IsSuccess)
                {
                    return BadRequest(seedingResult.Errors);
                }

                user = _context.Users
                    .Include(u => u.Categories)
                    .ThenInclude(c => c.Transactions)
                    .Where(u => u.Id == GetUserId())
                    .FirstOrDefault();
            }            

            return Ok(_mapper.Map<IEnumerable<CategoryViewModel>>(user.Categories));
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest categoryRequest)
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

            user.Categories.Add(category);
            var added = await _context.SaveChangesAsync() > 0;

            if (!added)
            {
                return BadRequest("Unable to create category");
            }

            var categoryResponse = _mapper.Map<CategoryViewModel>(category);

            return CreatedAtAction(nameof(CreateCategory), categoryResponse.Id, categoryResponse);
        }

        [HttpPatch]
        public async Task<ActionResult<CategoryViewModel>> UpdateCategory(UpdateCategoryRequest categoryRequest)
        {
            var user = _context.Users
                .Include(u => u.Categories)
                .Where(u => u.Id == GetUserId())
                .FirstOrDefault();

            if (user is null)
            {
                return BadRequest();
            }

            var category = user.Categories
                .Where(c => c.Id == categoryRequest.Id)
                .FirstOrDefault();

            _mapper.Map<UpdateCategoryRequest, Category>(categoryRequest, category);

            return await _context.SaveChangesAsync() > 0
                ? Ok(_mapper.Map<CategoryViewModel>(category))
                : NoContent();
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

            var category = user.Categories
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (category.IsBaseCategory)
            {
                return BadRequest("You cannot delete base categories");
            }

            user.Categories.Remove(category);

            return await _context.SaveChangesAsync() > 0
                ? Ok()
                : NoContent();
        }
    }
}
