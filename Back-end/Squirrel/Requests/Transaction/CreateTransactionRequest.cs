using System.ComponentModel.DataAnnotations;

namespace Squirrel.Requests.Transaction
{
    public class CreateTransactionRequest
    {
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
