using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Squirrel.Contexts;
using Squirrel.Entities;
using Squirrel.Requests.Category;

namespace Squirrel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public CategoryController(
            IMapper mapper,
            ApplicationContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
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
            return await _context.SaveChangesAsync() > 0
                ? CreatedAtAction(nameof(CreateCategory), category.Id, category)
                : BadRequest("Unable to create category");
        }

        private string GetUserId() => _userManager.GetUserId(User);
    }
}
