namespace DataAccess.Entities
{
    public class Subscription : UserRelatedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public double Price { get; set; }
        public TimeSpan Period { get; set; }

    }
}
