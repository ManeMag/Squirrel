namespace Squirrel.Models
{
    public class PiggyBank : UserRelatedModel
    {
        public PiggyBank(string name, double goal, User user) : base(user)
        {
            Name = name;
            Goal = goal;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; } = 0;
        public double Goal { get; set; }
    }
}
