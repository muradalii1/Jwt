using EventMiniApp.Dtos.Auth;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventMiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        // TEMP USER STORAGE (hələ DB yoxdur deyə)
        private static List<string> users = new();

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        // =====================
        // REGISTER
        // =====================
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (users.Contains(dto.Username))
                return BadRequest("User already exists");

            users.Add(dto.Username);

            return Ok("User created successfully");
        }

        // =====================
        // LOGIN
        // =====================
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (!users.Contains(dto.Username))
                return Unauthorized("User not found");

            var token = GenerateToken(dto.Username);

            return Ok(new { token });
        }

        // =====================
        // TOKEN GENERATOR
        // =====================
        private string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}