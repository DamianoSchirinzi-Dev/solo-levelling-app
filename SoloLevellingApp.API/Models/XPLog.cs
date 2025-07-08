using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class XPLog
    {
        public int Id { get; set; } // Unique identifier for the XP log entry

        [Required]
        public int UserId { get; set; } // Foreign key to User

        public int HabitId { get; set; } // Foreign key to Habit (if applicable)
 
        public int HabitCompletionId { get; set; } // Foreign key to HabitCompletion (if applicable)
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // Navigation property to User
        
        [ForeignKey(nameof(HabitId))]
        public Habit? Habit { get; set; } // Navigation property to Habit

        [ForeignKey(nameof(HabitCompletionId))]
        public HabitCompletion? HabitCompletion { get; set; } // Navigation
        
        [Required]        
        public int XPAmount { get; set; } // Amount of XP gained or lost

        [Required]
        public DateTime Timestamp { get; set; } // Timestamp of when the XP was logged

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // Description of the XP change (e.g., "Completed Habit", "Daily Login Bonus")

        [Range(0, int.MaxValue)]
        public int Streak { get; set; } // Current streak count, if applicable
        
        public XPLog()
        {
            Timestamp = DateTime.UtcNow; // Initialize timestamp to current UTC time
        }
    }
}
