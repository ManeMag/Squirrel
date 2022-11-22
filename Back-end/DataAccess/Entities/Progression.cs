namespace DataAccess.Entities
{
    public class Progression : UserRelatedEntity
    {
        public int Id { get; set; }
        public double Progress { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
