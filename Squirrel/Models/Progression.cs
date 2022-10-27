namespace Squirrel.Models
{
    public class Progression : UserRelatedModel
    {
        public Progression() { }
        public Progression(User user) : base(user){ }

        public int Id { get; set; }
        public double Progress { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
