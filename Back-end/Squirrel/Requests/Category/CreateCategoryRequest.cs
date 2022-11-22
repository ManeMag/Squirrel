using System.ComponentModel.DataAnnotations;

namespace Squirrel.Requests.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int Type { get; set; }
    }
}
