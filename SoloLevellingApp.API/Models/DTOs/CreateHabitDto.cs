using SoloLevellingApp.API.Models.Enums;

namespace SoloLevellingApp.API.Models.DTOs
{
    public class CreateHabitDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; } = HabitFrequency.Daily;
    }
}
