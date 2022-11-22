using System.ComponentModel.DataAnnotations;

namespace Squirrel.Requests.Category
{
    public class UpdateCategoryRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
