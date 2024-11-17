using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagerAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> SignUpAsync(ManagerSignupRequest request)
        {
            // בדיקת אם הדוא"ל כבר קיים
            var existingUser = await _context.Managers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already in use");
            }

            // 3. Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Hash the password
           // var hashedPassword = HashPassword(request.Password);

            // 4. Create new manager
            var manager = new Manager
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };
            // יצירת עובדים ברירת מחדל
            var defaultEmployees = new List<Employee>
        {
            new Employee
            {
                FullName = "Default Employee 1",
                Email = "employee1@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password1"), // Hash לסיסמה
                ManagerId = manager.Id
            },
            new Employee
            {
                FullName = "Default Employee 2",
                Email = "employee2@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password2"), // Hash לסיסמה
                ManagerId = manager.Id
            }
        };

            // הוספת המנהל והעובדים ל-DB
            manager.Employees = defaultEmployees;
           // _dbContext.Managers.Add(manager);
            // הוספת המשתמש החדש לבסיס הנתונים
            await _context.Managers.AddAsync(manager);
            await _context.SaveChangesAsync();

            // החזרת טוקן JWT למקרה הצורך
            return "true";
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest user)
        {
            Console.WriteLine(user);
            var existingUser = await _context.Managers.FirstOrDefaultAsync(u => u.Email == user.Email);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, hashedPassword))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            return new LoginResponse(GenerateJwtToken(existingUser), existingUser);
        }

        private string GenerateJwtToken(BaseEntity user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, (user.Id).ToString())
            
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyWithEnoughLength384Bits"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5009",
                audience: "http://localhost:5009",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
