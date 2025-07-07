using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Helpers
{
    public static class ApiHelpers
    {
        public static int CalculateStreak(List<HabitCompletion> completions)
        {
            var streak = 0;
            var current = DateTime.UtcNow.Date;

            foreach (var day in completions.Select(c => c.CompletedAt.Date).Distinct())
            {
                if (day == current)
                {
                    streak++;
                    current = current.AddDays(-1);
                }
                else break;
            }

            return streak;
        }
    }
}
