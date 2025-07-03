using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class XPLog
    {
        [ForeignKey("XPLogId")]
        public int Id { get; set; } // Unique identifier for the XP log entry
        public int UserId { get; set; } // Foreign key to User
        public int HabitId { get; set; } // Foreign key to Habit (if applicable)
        public int HabitCompletionId { get; set; } // Foreign key to HabitCompletion (if applicable)
        public User User { get; set; } // Navigation property to User
        public int XPAmount { get; set; } // Amount of XP gained or lost
        public DateTime Timestamp { get; set; } // Timestamp of when the XP was logged
        public string Description { get; set; } // Description of the XP change (e.g., "Completed Habit", "Daily Login Bonus")
        public int Streak { get; set; } // Current streak count, if applicable
        public XPLog()
        {
            Timestamp = DateTime.UtcNow; // Initialize timestamp to current UTC time
        }
    }
}
