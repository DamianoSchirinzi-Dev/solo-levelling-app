using System.ComponentModel.DataAnnotations.Schema;

namespace SoloLevellingApp.API.Models
{
    public class Habit
    {
        [ForeignKey("HabitId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public enum Frequency
        {
            Daily,
            Weekly,
            Monthly
        }
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
