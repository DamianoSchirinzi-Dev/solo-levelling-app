using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Services
{
    public class DailyResetService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public DailyResetService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(1).AddHours(0); 
            var timeToGo = nextRun - now;

            _timer = new Timer(DoWork, null, timeToGo, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            ResetDailyHabits(db);
            UpdateStreaks(db);

            db.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void ResetDailyHabits(AppDbContext db)
        {
            // Optionally add logs or user notifications here
            // Not strictly required if you use HabitCompletions only
        }

        private void UpdateStreaks(AppDbContext db)
        {
            var today = DateTime.UtcNow.Date;
            var yesterday = today.AddDays(-1);

            // Get all habits with their completions in a single query
            var habitsWithCompletions = db.Habits
                .Select(h => new
                {
                    Habit = h,
                    CompletedToday = db.HabitCompletions.Any(hc =>
                        hc.UserId == h.UserId &&
                        hc.HabitId == h.Id &&
                        hc.CompletedAt.Date == today),
                    User = h.User
                })
                .ToList();

            foreach (var item in habitsWithCompletions)
            {
                var habit = item.Habit;
                var user = item.User;

                if (item.CompletedToday)
                {
                    // Update streak
                    if (habit.LastCompletedAt?.Date == yesterday)
                    {
                        habit.CurrentStreak += 1;
                    }
                    else if (habit.LastCompletedAt?.Date != today)
                    {
                        habit.CurrentStreak = 1;
                    }

                    habit.LastCompletedAt = today;

                    // Optional: give XP bonus for reaching milestone streaks
                    if (habit.CurrentStreak % 7 == 0) // e.g., every 7 days
                    {
                        db.XPLogs.Add(new XPLog
                        {
                            UserId = user.Id,
                            HabitId = habit.Id,
                            XPAmount = 50,
                            Timestamp = DateTime.UtcNow,
                            Description = $"Streak Bonus: {habit.CurrentStreak} days",
                            Streak = habit.CurrentStreak
                        });

                        user.XP += 50;
                        // Handle level ups
                        while (user.XP >= user.Level * 100)
                        {
                            user.XP -= user.Level * 100;
                            user.Level += 1;
                        }
                    }
                }
                else
                {
                    // Missed a day – reset streak
                    habit.CurrentStreak = 0;
                    habit.LastCompletedAt = null;
                }
            }
        }
    }
}
