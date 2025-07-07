using Npgsql;
using SoloLevellingApp.API.Data;
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

            user.XP = xpAmount;
            if(user.XP >= user.Level * 100)
            {
                user.Level += 1;
                user.XP = 0;
            }

            var log = new XPLog
            {
                UserId = userId,
                HabitId = habitId,
                HabitCompletionId = habitCompletionId,
                XPAmount = xpAmount,
                Timestamp = DateTime.UtcNow,
                Description = reason ?? "Completed habit",
                Streak = 0 // optional: plug in streak service here
            };

            _context.XPLogs.Add(log);
            _context.SaveChanges();
        }
    }
}
