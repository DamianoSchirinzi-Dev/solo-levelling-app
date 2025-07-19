using SoloLevellingApp.API.Models.Enums;

namespace SoloLevellingApp.API.Models.DTOs
{
    public class CreateHabitDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; } = HabitFrequency.Daily;
        public string Category { get; set; } = "Other"; 
        public Difficulty Difficulty { get; set; } = Difficulty.Medium; 
        public int XpValue { get; set; } = 10; 
        public bool ReminderEnabled { get; set; } = false;
        public string ReminderTime { get; set; } = "09:00"; 
    }
}
