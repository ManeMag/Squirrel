using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Squirrel.Entities;

namespace Squirrel.Contexts
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<PiggyBank> PiggyBanks { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Progression> Progressions { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.EnsureCreated();
    }
}