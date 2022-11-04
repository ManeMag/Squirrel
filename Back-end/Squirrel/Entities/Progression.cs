namespace Squirrel.Entities
{
    public class Progression : UserRelatedModel
    {
        public int Id { get; set; }
        public double Progress { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
