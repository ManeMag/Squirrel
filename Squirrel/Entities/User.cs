using Microsoft.AspNetCore.Identity;
using Squirrel.Models;

namespace Squirrel.Entities
{
    public class User : IdentityUser
    {
        public User() { }

        public User(string email) => UserName = Email = email;
         
        public User(RegisterModel model) => UserName = Email = model.Email;
        public DateTime ExpirationDate { get; set; } = DateTime.MinValue;
        public string Currency { get; set; } = "$";
        public List<Progression>? Progressions { get; set; }
        public List<Subscription>? Subscriptions { get; set; }
        public List<PiggyBank>? PiggyBanks { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
