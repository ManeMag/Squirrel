namespace Squirrel.Models
{
    public class Transaction
    {
        public Transaction(DateTime time, double amount, string description, Category category)
        {
            Time = time;
            Amount = amount;
            Description = description;
            Category = category;
            CategoryId = category.Id;
        }

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
