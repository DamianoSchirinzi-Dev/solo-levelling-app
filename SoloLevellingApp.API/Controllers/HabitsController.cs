using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Models;
using SoloLevellingApp.API.Models.DTOs;
using SoloLevellingApp.API.Models.Enums;
using System.Security.Claims;

namespace SoloLevellingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HabitsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public HabitsController(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetHabits()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var today = DateTime.UtcNow.Date;

            var habits = _appDbContext.Habits
                .Where(h => h.UserId == userId)
                .Select(h => new HabitDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt,
                    XPValue = h.XPValue,
                })
                .ToList();

            return Ok(habits);
        }

        [HttpGet("{id}")]
        public IActionResult GetHabit(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var today = DateTime.UtcNow.Date;

            var habit = _appDbContext.Habits
                .Where(h => h.Id == id && h.UserId == userId)
                .Select(h => new HabitDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt,
                    XPValue = h.XPValue,
                })
                .FirstOrDefault();

            if (habit == null) return NotFound();
            return Ok(habit);
        }

        [HttpGet("today")]
        public IActionResult GetHabitsFromToday()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var today = DateTime.UtcNow.Date;
            var todayDayOfWeek = today.DayOfWeek;
            var todayDayOfMonth = today.Day;

            var habits = _appDbContext.Habits
                .Where(h => h.UserId == userId)
                .AsEnumerable()
                .Where(h =>
                    h.Frequency == HabitFrequency.Daily ||
                    (h.Frequency == HabitFrequency.Weekly && h.CreatedAt.DayOfWeek == todayDayOfWeek) ||
                    (h.Frequency == HabitFrequency.Monthly && h.CreatedAt.Day == todayDayOfMonth)
                )
                .Select(h => new HabitDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt,
                    XPValue = h.XPValue
                })
                .ToList();

            return Ok(habits);
        }

        [HttpPost("create")]
        public IActionResult CreateHabit(CreateHabitDto dto) 
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var habit = new Habit
            {
                Name = dto.Name,
                Description = dto.Description,
                Frequency = dto.Frequency,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _appDbContext.Habits.Add(habit);
            _appDbContext.SaveChanges();

            var habitDto = new HabitDto
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                Frequency = habit.Frequency,
                CreatedAt = habit.CreatedAt
            };

            return Ok(habitDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHabit(int id, CreateHabitDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var habit = _appDbContext.Habits.FirstOrDefault(h => h.Id == id && h.UserId == userId);

            if (habit == null) return NotFound();

            habit.Name = dto.Name;
            habit.Description = dto.Description;
            habit.Frequency = dto.Frequency;
            habit.UpdatedAt = DateTime.UtcNow;

            _appDbContext.SaveChanges();

            var habitDto = new HabitDto
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                Frequency = habit.Frequency,
                CreatedAt = habit.CreatedAt
            };

            return Ok(habitDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHabit(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var habit = _appDbContext.Habits.FirstOrDefault(h => h.Id == id && h.UserId == userId);

            if (habit == null) return NotFound();  

            _appDbContext.Habits.Remove(habit);
            _appDbContext.SaveChanges();

            return NoContent();
        }
    }
}
