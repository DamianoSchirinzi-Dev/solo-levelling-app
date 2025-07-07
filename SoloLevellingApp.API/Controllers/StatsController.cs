using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Helpers;
using System.Security.Claims;

namespace SoloLevellingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatsController: ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public StatsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetStats()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _appDbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) return NotFound();

            var completions = _appDbContext.HabitCompletions
                .Where(h => h.UserId == userId)
                .OrderByDescending(c => c.CompletedAt)
                .ToList();

            var streak = ApiHelpers.CalculateStreak(completions);

            return Ok(new
            {
                user.XP,
                user.Level,
                CurrentStreak = streak
            });
        }
    }
}
