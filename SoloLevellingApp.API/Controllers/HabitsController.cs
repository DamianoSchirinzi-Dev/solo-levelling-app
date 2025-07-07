using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Models;
using SoloLevellingApp.API.Models.DTOs;
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
            var habits = _appDbContext.Habits
                .Where(h => h.UserId == userId)
                .Select(h => new HabitDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Description = h.Description,
                    CreatedAt = DateTime.UtcNow,
                })
                .ToList();

            return Ok(habits);
        }

        [HttpPost]
        public IActionResult CreateHabit(CreateHabitDto dto) 
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var habit = new Habit
            {
                Name = dto.Name,
                Description = dto.Description,
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
            habit.UpdatedAt = DateTime.UtcNow;

            _appDbContext.SaveChanges();

            var habitDto = new HabitDto
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
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
