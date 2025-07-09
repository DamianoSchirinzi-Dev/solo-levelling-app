using SoloLevellingApp.API.Models.Enums;

namespace SoloLevellingApp.API.Models.DTOs
{
    public class HabitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int XPValue { get; set; }
    }
}
