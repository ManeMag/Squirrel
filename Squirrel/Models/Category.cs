namespace Squirrel.Models
{
    public class Category : UserRelatedModel
    {
        public Category(string name, string color, User user) : base(user)
        {
            Name = name;
            Color = color;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}
