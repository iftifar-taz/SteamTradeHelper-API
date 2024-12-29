using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Utilities;

namespace SteamTradeHelper.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Bot> Bots { get; set; }

        /// <inheritdoc/>
        public override int SaveChanges()
        {
            UpdateCreatedAndUpdatedBy();
            UpdateUpdatedAt();
            return base.SaveChanges();
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateCreatedAndUpdatedBy();
            UpdateUpdatedAt();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>().HasIndex(u => u.AppId).IsUnique();

            builder.Entity<Card>().HasIndex(u => u.ItemId).IsUnique();
            builder.Entity<Card>().HasOne(x => x.Game).WithMany(x => x.Cards).HasForeignKey(x => x.GameId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Bot>().HasIndex(u => u.SteamId).IsUnique();
        }

        private void UpdateCreatedAndUpdatedBy()
        {
            var user = GlobalConstants.DefaultUser; //TO:DO
            user ??= GlobalConstants.DefaultUser;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.CurrentValues["CreatedBy"] = user;
                }

                entry.CurrentValues["UpdatedBy"] = user;
            }
        }

        private void UpdateUpdatedAt()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
