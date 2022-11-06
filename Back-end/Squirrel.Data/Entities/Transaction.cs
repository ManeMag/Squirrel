namespace Squirrel.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
