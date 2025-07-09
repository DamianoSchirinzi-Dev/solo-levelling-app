using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Services;
using System.Security.Claims;

namespace SoloLevellingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly XPService _xpService;

        public UserController(AppDbContext context, XPService xpService)
        {
            _context = context;
            _xpService = xpService;
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) return NotFound();

            _xpService.RefreshUserProgress(user.Id);

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Title,
                user.Level,
                user.XP,
                user.XPtoNextLevel,
                user.Streak,
                user.CreatedAt,
                user.LastLogin
            });
        }
    }
}
