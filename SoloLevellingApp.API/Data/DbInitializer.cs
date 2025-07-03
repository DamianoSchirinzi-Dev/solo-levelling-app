using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Data
{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any()) return; // Skip if already seeded

            var user = new User
            {
                Username = "demo_user",
                Email = "demo@example.com",
                PasswordHash = "hashed-password", // Replace with actual hash
                XP = 20,
                Level = 1,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            var habit1 = new Habit
            {
                Name = "Drink Water",
                Description = "Drink 2L of water",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                User = user
            };

            var habit2 = new Habit
            {
                Name = "Exercise",
                Description = "30 minutes of physical activity",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                User = user
            };

            var completion1 = new HabitCompletion
            {
                Habit = habit1,
                User = user,
                CompletedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            context.Habits.AddRange(habit1, habit2);
            context.HabitCompletions.Add(completion1);

            context.SaveChanges();

            var xplog = new XPLog
            {
                User = user,
                HabitId = habit1.Id,
                XPAmount = 10,
                Timestamp = DateTime.UtcNow,
                Description = "Completed habit: Drink Water"
            };

            context.XPLogs.Add(xplog);
            context.SaveChanges();
        }
    }
}
