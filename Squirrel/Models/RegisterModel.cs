using System.ComponentModel.DataAnnotations;

namespace Squirrel.Models
{
    public class RegisterModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? ConfirmPassword { get; set; }

        public bool Same => Password == ConfirmPassword;
    }
}
