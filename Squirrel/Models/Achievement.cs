    namespace Squirrel.Models
{
    public class Achievement
    {
        public Achievement() { }
        public Achievement(string name, string description, double goal)
        {
            Name = name;
            Description = description;
            Goal = goal;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Goal { get; set; }
    }
}
