namespace DataAccess.Entities
{
    public class PiggyBank : UserRelatedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Goal { get; set; }
    }
}
