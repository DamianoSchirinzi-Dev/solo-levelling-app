using Microsoft.EntityFrameworkCore;
using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitCompletion> HabitCompletions { get; set; }
        public DbSet<XPLog> XPLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HabitCompletion>()
                .HasIndex(hc => new { hc.UserId, hc.HabitId, hc.CompletedAt })
                .IsUnique();

            // XPLog relationships with nullable foreign keys
            modelBuilder.Entity<XPLog>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<XPLog>()
                .HasOne(x => x.Habit)
                .WithMany()
                .HasForeignKey(x => x.HabitId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<XPLog>()
                .HasOne(x => x.HabitCompletion)
                .WithMany()
                .HasForeignKey(x => x.HabitCompletionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Add indexes for performance
            modelBuilder.Entity<XPLog>()
                .HasIndex(x => x.UserId);

            modelBuilder.Entity<XPLog>()
                .HasIndex(x => x.Timestamp);

            modelBuilder.Entity<Habit>()
                .HasIndex(h => h.UserId);
        }
    }
}
