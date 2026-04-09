using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartParking.API.Data;
using SmartParking.API.DTOs;
using SmartParking.API.Models;
using SmartParking.API.Services;

namespace SmartParking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        
        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);
            
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });
            
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });
            
            var token = _jwtService.GenerateToken(user.UserId, user.Username, user.Role);
            
            return Ok(new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                UserId = user.UserId
            });
        }
    }
}