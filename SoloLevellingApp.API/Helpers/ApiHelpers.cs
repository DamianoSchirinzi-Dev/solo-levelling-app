using SoloLevellingApp.API.Models;

namespace SoloLevellingApp.API.Helpers
{
    public static class ApiHelpers
    {
        public static int CalculateStreak(List<HabitCompletion> completions)
        {
            if (completions == null || !completions.Any())
                return 0;

            var streak = 0;
            var today = DateTime.UtcNow.Date;

            // Extract distinct days when the user completed *any* habit
            var daysWithCompletions = completions
                .Select(c => c.CompletedAt.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            // Iterate day-by-day from today backwards
            for (int i = 0; i < daysWithCompletions.Count; i++)
            {
                if (daysWithCompletions[i] == today.AddDays(-streak))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }
    }
}
