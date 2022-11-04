namespace Squirrel.Entities
{
    public class Subscription : UserRelatedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public double Price { get; set; }
        public TimeSpan Period { get; set; }

    }
}
