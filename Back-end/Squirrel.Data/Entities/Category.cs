namespace DataAccess.Entities
{
    public class Category : UserRelatedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
        public bool IsBaseCategory { get; set; }
    }
}
