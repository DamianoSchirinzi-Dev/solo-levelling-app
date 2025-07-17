using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Models;
using SoloLevellingApp.API.Models.DTOs;
using SoloLevellingApp.API.Services;
using System.Security.Claims;

namespace SoloLevellingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompletionsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly XPService _xpService;

        public CompletionsController(AppDbContext appDbContext, XPService xpService)
        {
            _appDbContext = appDbContext;
            _xpService = xpService;
        }

        [HttpGet("user-completions")]
        public IActionResult GetTodayUserCompletions()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var today = DateTime.UtcNow;

            var completions = _appDbContext.HabitCompletions.Where(h => h.UserId == userId && 
                h.CompletedAt.Date == today.Date)
                .Select(c => new HabitCompletionDto
                {
                    Id = c.Id,
                    HabitId = c.HabitId,
                    CompletedAt = c.CompletedAt
                })
                .ToList();


            if (!completions.Any()) return NotFound("No Completions found for today");

            return Ok(completions);
        }

        [HttpPost("{habitId}/complete")]
        public IActionResult CompleteHabit(int habitId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var habit = _appDbContext.Habits
                .FirstOrDefault(h => h.Id == habitId && h.UserId == userId);

            if(habit == null) return NotFound("Habit not found");

            var alreadyCompleted = _appDbContext.HabitCompletions.Any(c => 
                c.HabitId == habitId && 
                c.UserId == userId &&
                c.CompletedAt.Date == DateTime.UtcNow.Date);

            if (alreadyCompleted) return BadRequest("Habit already completed today.");

            var completion = new HabitCompletion
            {
                UserId = userId,
                HabitId = habitId,
                CompletedAt = DateTime.UtcNow
            };

            _appDbContext.HabitCompletions.Add(completion);
            _appDbContext.SaveChanges();

            _xpService.AwardXP(
                userId,
                habitId,
                completion.Id,
                xpAmount: 10,
                reason: $"Completed habit: {habit.Name}"
            );

            return Ok(new HabitCompletionDto
            {
                Id = completion.Id,
                HabitId = completion.HabitId,
                CompletedAt = completion.CompletedAt,
            });
        }

        [HttpDelete("{habitId}/uncomplete")]
        public IActionResult UncompleteHabit(int habitId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var completion = _appDbContext.HabitCompletions
                .Where(c =>
                    c.HabitId == habitId &&
                    c.UserId == userId &&
                    c.CompletedAt.Date == DateTime.UtcNow.Date)
                .OrderByDescending(c => c.CompletedAt)
                .FirstOrDefault();

            if (completion == null)
                return NotFound("No completion found for today to uncomplete.");

            _appDbContext.HabitCompletions.Remove(completion);
            _appDbContext.SaveChanges();

            _xpService.RefreshUserProgress(userId);

            return NoContent();
        }
    }
}
