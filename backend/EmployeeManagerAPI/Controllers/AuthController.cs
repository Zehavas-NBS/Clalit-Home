using EmployeeManagerAPI.Models.Requests;
using EmployeeManagerAPI.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       
        private readonly ILog _logger;

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _logger = LogManager.GetLogger(typeof(AuthController));
            _authService = authService;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] ManagerSignupRequest signupRequest)
        {
            try
            {
                _logger.Info("Signup request received.");

                if (signupRequest == null)
                {
                    _logger.Warn("Signup request body is null.");
                    return BadRequest(new { error = "Invalid signup data." });
                }
                var result = await _authService.SignUpAsync(signupRequest);
                if (result == null)
                {
                    _logger.Warn("Signup failed.");
                    return BadRequest(new { error = "Signup failed. Please try again." });
                }
                _logger.Info($"Signup successful for user: {signupRequest.Email}");
                return CreatedAtAction(nameof(SignUp), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                _logger.Error("Error occurred during signup.", ex);
                return StatusCode(500, new { error = string.Format("An error occurred while processing the signup request.{0}",ex.Message) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred during signup.", ex);
                return StatusCode(500, new { error = "An error occurred while processing the signup request." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInRequest request)
        {
            try
            {
                _logger.Info("Login request received.");

                if (request == null)
                {
                    _logger.Warn("Login request body is null.");
                    return BadRequest(new { error = "Invalid login data." });
                }

                var result = await _authService.LoginAsync(request);
                if (result == null)
                {
                    _logger.Warn("Invalid login credentials provided.");
                    return Unauthorized(new { error = "Invalid email or password." });
                }

                _logger.Info($"Login successful for user: {request.Email}");

                return Ok(new { result.Token , result.ManagerData });
            }
          
            catch (Exception ex)
            {
                _logger.Error("Error occurred during login.", ex);
                return StatusCode(500, new { error = "An error occurred while processing the login request." });
            }
        } 
    }
}
