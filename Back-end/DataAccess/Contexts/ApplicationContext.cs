using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DataAccess.Contexts
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .HasIndex(p => new { p.Name, p.UserId }).IsUnique();
            base.OnModelCreating(builder);
        }
    }
}