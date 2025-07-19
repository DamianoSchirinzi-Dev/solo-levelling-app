using SoloLevellingApp.API.Models;
using SoloLevellingApp.API.Models.Enums;

namespace SoloLevellingApp.API.Models.DTOs
{
    public class HabitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }
        public string Category { get; set; } = "Other"; 
        public int XPValue { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool ReminderEnabled { get; set; }
        public string ReminderTime { get; set; } = "09:00";
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}