using SoloLevellingApp.API.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class Habit
    {
        [ForeignKey("HabitId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; } 
        public Difficulty Difficulty { get; set; } 
        public string Category { get; set; } = "Other"; //Make enum
        public int CurrentStreak { get; set; }
        public int XpValue { get; set; } 
        public bool ReminderEnabled { get; set; }
        public string ReminderTime { get; set; }
        public DateTime? LastCompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public User User { get; set; } // Navigation property

        public Habit()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
