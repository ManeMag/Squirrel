using System.ComponentModel.DataAnnotations;

namespace Squirrel.Models
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? Code { get; set; }

        public bool Same => Password == ConfirmPassword;
    }
}
