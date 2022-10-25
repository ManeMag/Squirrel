namespace Squirrel.Models
{
    public class Subscription : UserRelatedModel
    {
        public Subscription() { }
        public Subscription(string name, DateTime day, double price, TimeSpan period, User user) : base(user)
        {
            Name = name;
            Day = day;
            Price = price;
            Period = period;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public double Price { get; set; }
        public TimeSpan Period { get; set; }

    }
}
