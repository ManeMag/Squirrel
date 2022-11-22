using System.ComponentModel.DataAnnotations;

namespace Squirrel.Requests.Transaction
{
    public class UpdateTransactionRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
