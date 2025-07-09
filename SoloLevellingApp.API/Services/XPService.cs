using Npgsql;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Helpers;
using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Services
{
    public class XPService
    {
        private readonly AppDbContext _context;

        public XPService(AppDbContext context)
        {
            _context = context;
        }

        public void AwardXP(int userId, int habitId, int habitCompletionId, int xpAmount, string reason = "")
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return;

            user.XP += xpAmount;
            if(user.XP >= user.Level * 100)
            {
                user.XP -= user.Level * 100;
                user.Level += 1;
            }

            // Calculate current streak for logging
            var completions = _context.HabitCompletions
                .Where(h => h.UserId == userId)
                .OrderByDescending(c => c.CompletedAt)
                .ToList();

            var currentStreak = ApiHelpers.CalculateStreak(completions);

            RefreshUserProgress(user.Id, completions);

            var log = new XPLog
            {
                UserId = userId,
                HabitId = habitId,
                HabitCompletionId = habitCompletionId,
                XPAmount = xpAmount,
                Timestamp = DateTime.UtcNow,
                Description = reason ?? "Completed habit",
                Streak = currentStreak
            };

            _context.XPLogs.Add(log);
            _context.SaveChanges();
        }

        public void RefreshUserProgress(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return;

            // Update XP to next level
            user.XPtoNextLevel = user.Level * 100;

            // Recalculate streak from completions
            var completions = _context.HabitCompletions
                .Where(h => h.UserId == userId)
                .OrderByDescending(c => c.CompletedAt)
                .ToList();

            user.Streak = ApiHelpers.CalculateStreak(completions);

            _context.SaveChanges();
        }

        public void RefreshUserProgress(int userId, List<HabitCompletion> completions)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return;

            // Update XP to next level
            user.XPtoNextLevel = user.Level * 100;

            user.Streak = ApiHelpers.CalculateStreak(completions);

            _context.SaveChanges();
        }
    }
}
