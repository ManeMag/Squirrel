namespace DataAccess.Entities
{
    public enum Type { Income = 1, Outcome = 2, Both = 3 }
    public class Category : UserRelatedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Type Type { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
        public bool IsBaseCategory { get; set; }

    }
}
