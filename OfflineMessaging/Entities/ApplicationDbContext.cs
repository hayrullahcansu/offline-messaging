using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Utils;


namespace OfflineMessaging.Entities
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserBlock> UserBlocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
            optionsBuilder.EnableSensitiveDataLogging();
        }

        private static string GetConnectionString()
        {
            return Constants.ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Message>().Property(x => x.Text).HasColumnType("varchar(2000)");


            builder.Entity<Message>().Property(x => x.Text).HasColumnType("varchar(2000)");


            builder.Entity<UserActivity>().Property(x => x.Operation).HasColumnType("varchar(100)");
            builder.Entity<UserActivity>().Property(x => x.Text).HasColumnType("varchar(2000)");


            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().Property(x => x.VerifyState).HasColumnType("varchar(20)");


            //builder.Entity<Currency>().Property(x => x.TxFee).HasColumnType("decimal(12,4)").HasDefaultValue(0M);

            // identity stuff
            builder.Entity<IdentityRole>().ToTable("Roles");

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker
                .Entries()
                .Where(
                    x =>
                        x.Entity is TimeAwareEntity &&
                        (x.State == EntityState.Added || x.State == EntityState.Modified)
                );

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((TimeAwareEntity) entity.Entity).DateCreated = DateTime.UtcNow;
                }

                ((TimeAwareEntity) entity.Entity).DateModified = DateTime.UtcNow;
            }
        }
    }
}