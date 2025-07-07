namespace SoloLevellingApp.API.Models.DTOs
{
    public class HabitCompletionDto
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
