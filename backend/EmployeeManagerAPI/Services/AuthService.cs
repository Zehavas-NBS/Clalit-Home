using EmployeeManagerAPI.Data;
using EmployeeManagerAPI.Models;
using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Models.Responses;
using EmployeeManagerAPI.Validations;
using log4net;
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
        private readonly ILog _logger;


        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _logger = LogManager.GetLogger(typeof(AuthService));

            _configuration = configuration;
            _context = context;
        }

        public async Task<IEmployee> SignUpAsync(ManagerSignupRequest request)
        {
            try
            {
                _logger.Info($"Starting registration for email: {request.Email}");

                // Validate email format
                if (string.IsNullOrWhiteSpace(request.Email) || !ValidationHelper.IsValidEmail(request.Email))
                {
                    _logger.Warn($"Invalid email format: {request.Email}");
                    throw new ArgumentException(ErrorMessages.InvalidEmail);
                }

                // Validate password strength
                if (string.IsNullOrWhiteSpace(request.Password) || !ValidationHelper.IsValidPassword(request.Password))
                {
                    _logger.Warn($"Weak or invalid password provided for email: {request.Email}");
                    throw new ArgumentException(ErrorMessages.InvalidPassword);
                }

                // Validate full name
                if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length < 2)
                {
                    _logger.Warn($"Invalid full name provided: {request.FullName}");
                    throw new ArgumentException(ErrorMessages.InvalidFullName);

                }
                // בדיקת אם הדוא"ל כבר קיים
                var existingUser = await _context.Managers.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    _logger.Warn($"Email already exists: {request.Email}");
                    throw new ArgumentException(ErrorMessages.EmailAlreadyExists);
                }

                // 3. Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            

                // 4. Create new manager
                var manager = new Manager(request.Email, request.FullName, hashedPassword);
               
                // יצירת עובדים ברירת מחדל
                var defaultEmployees = new List<Employee>
        {
            new Employee("employee1@example.com",  "Default Employee 1",BCrypt.Net.BCrypt.HashPassword("password1"),manager.Id),
            new Employee("employreqree2@example.com", "Default Employee 2",BCrypt.Net.BCrypt.HashPassword("password2"),manager.Id),
             new Employee("employee1@example.com",  "Default Employee 3",BCrypt.Net.BCrypt.HashPassword("password1"),manager.Id),
            new Employee("employreqree2@example.com", "Default Employee 4",BCrypt.Net.BCrypt.HashPassword("password2"),manager.Id),
             new Employee("employee1@example.com",  "Default Employee 5",BCrypt.Net.BCrypt.HashPassword("password1"),manager.Id),
            new Employee("employreqree2@example.com", "Default Employee 6",BCrypt.Net.BCrypt.HashPassword("password2"),manager.Id)
        };

                // הוספת המנהל והעובדים ל-DB
                manager.Employees = defaultEmployees;
                // _dbContext.Managers.Add(manager);
                // הוספת המשתמש החדש לבסיס הנתונים
                await _context.Managers.AddAsync(manager);
                await _context.SaveChangesAsync();
                _logger.Info($"User registered successfully with email: {request.Email}");

                // החזרת טוקן JWT למקרה הצורך
                return manager;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during registration for email: {request.Email}", ex);
                throw;
            }
        }

        public async Task<LoginResponse> LoginAsync(LogInRequest request)
        {
            try
            {
                _logger.Info($"Authenticating user with email: {request.Email}");

                var existingUser = await _context.Managers.FirstOrDefaultAsync(u => u.Email == request.Email);

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                if (existingUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, existingUser.Password))
                {
                    _logger.Warn($"Authentication failed for email: {request.Email}");

                    return null;
                }

                var token = GenerateJwtToken(existingUser);
                _logger.Info($"Authentication successful for email: {request.Email}");
                return new LoginResponse(token, existingUser);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during authentication for email: {request.Email}", ex);
                throw;
            }

        }

        private string GenerateJwtToken(IEmployee user)
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
