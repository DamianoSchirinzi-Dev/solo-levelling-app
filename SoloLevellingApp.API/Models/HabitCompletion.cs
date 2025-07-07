using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class HabitCompletion
    {
        [ForeignKey("HabitCompletionId")]
        public int Id { get; set; }
        public DateTime CompletedAt { get; set; } // Timestamp of when the habit was completed
        public int HabitId { get; set; } // Foreign key to Habit
        public Habit Habit { get; set; } // Navigation property to Habit
        public int UserId { get; set; } // Foreign key to User
        public HabitCompletion()
        {
            CompletedAt = DateTime.UtcNow;
        }
    }
}
