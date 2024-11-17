using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //private readonly AppDbContext _context;

        //public ManagersController(AppDbContext context)
        //{
        //    _context = context;
        //}

        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] ManagerSignupRequest user)
        {
            try
            {
                var token = await _authService.SignUpAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(new { response.Token ,response.ManagerData });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

  


        //[HttpPost("signup")]
        //public async Task<IActionResult> Signup([FromBody] ManagerSignupRequest request)
        //{
        //    // 1. Validate input
        //    if (string.IsNullOrEmpty(request.FullName) ||
        //        string.IsNullOrEmpty(request.Email) ||
        //        string.IsNullOrEmpty(request.Password))
        //    {
        //        return BadRequest("All fields are required.");
        //    }

        //    if (!request.Email.Contains("@"))
        //    {
        //        return BadRequest("Invalid email format.");
        //    }

        //    // 2. Check if manager already exists
        //    var existingManager = await _context.Managers
        //        .FirstOrDefaultAsync(m => m.Email == request.Email);

        //    if (existingManager != null)
        //    {
        //        return Conflict("A manager with this email already exists.");
        //    }

        //    // 3. Hash the password
        //    var hashedPassword = HashPassword(request.Password);

        //    // 4. Create new manager
        //    var manager = new Manager
        //    {
        //        FullName = request.FullName,
        //        Email = request.Email,
        //        Password = hashedPassword,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _context.Managers.Add(manager);
        //    await _context.SaveChangesAsync();

        //    return Ok("Manager registered successfully.");
        //}

        //private string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        return Convert.ToBase64String(hashedBytes);
        //    }
        //}
    }
}
