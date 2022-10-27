namespace Squirrel.Models
{
    public class UserRelatedModel
    {
        public UserRelatedModel()
        {
        }

        public UserRelatedModel(User user)
        {
            User = user;
            UserId = user.Id;
        }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
