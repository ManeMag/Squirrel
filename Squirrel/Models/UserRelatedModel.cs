namespace Squirrel.Models
{
    public class UserRelatedModel
    {
        public UserRelatedModel(User user)
        {
            User = user;
            UserId = user.Id;
        }

        public UserRelatedModel(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
