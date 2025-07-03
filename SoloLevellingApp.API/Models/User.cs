using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class User
    {
        [ForeignKey("UserId")]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }
        public List<Habit> Habits { get; set; } = new List<Habit>();
        public List<HabitCompletion> HabitCompletions { get; set; } = new List<HabitCompletion>();

        public User()
        {
            CreatedAt = DateTime.UtcNow;
            LastLogin = DateTime.UtcNow;
        }
    }
}
