namespace DataAccess.Entities
{
    public class PiggyBank : UserRelatedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Goal { get; set; }
    }
}
