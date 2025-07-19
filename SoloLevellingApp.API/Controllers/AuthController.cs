using Microsoft.AspNetCore.Mvc;
using SoloLevellingApp.API.Data;
using SoloLevellingApp.API.Models;
using SoloLevellingApp.API.Models.DTOs;
using SoloLevellingApp.API.Services;
using System.Runtime.CompilerServices;

namespace SoloLevellingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AppDbContext _context;

        public AuthController(AuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            if(_context.Users.Any(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                return new BadRequestObjectResult("Username or Email already exists.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _authService.HashPassword(registerDto.Password),
                XP = 0,
                Level = 1,
                Title = "Recruit"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Token = _authService.GenerateJwtToken(user) });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == loginDto.Identifier || u.Email == loginDto.Identifier);

            if (user == null || !_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            user.LastLogin = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(new {Token = _authService.GenerateJwtToken(user) });
        }
    }
}
