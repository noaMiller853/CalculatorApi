using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Authentication
{
    /// <summary>
    /// Controller Pattern - Controller for handling authentication logic
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly ILogger<AuthController> _logger;

        // Simple in-memory demo user store
        private static readonly Dictionary<string, string> _demoUsers = new()
        {
            { "admin", "1234" },
            { "test", "password" }
        };

        /// <summary>
        /// Constructor Injection Pattern - Injecting dependencies via constructor
        /// </summary>
        public AuthController(JwtTokenGenerator tokenGenerator, ILogger<AuthController> logger)
        {
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful
        /// </summary>
        /// <param name="loginRequest">Login credentials (username and password)</param>
        /// <response code="200">Returns JWT token if credentials are valid</response>
        /// <response code="401">Unauthorized - invalid credentials</response>
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest?.Username) || string.IsNullOrWhiteSpace(loginRequest?.Password))
                    return BadRequest("Username and password are required");

                // Validate credentials (hardcoded demo users)
                if (!_demoUsers.TryGetValue(loginRequest.Username, out var storedPassword) ||
                    storedPassword != loginRequest.Password)
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", loginRequest.Username);
                    return Unauthorized("Invalid username or password");
                }

                string token = _tokenGenerator.GenerateToken(loginRequest.Username);

                _logger.LogInformation("Generated token for user: {Username}", loginRequest.Username);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token");
                return StatusCode(500, "An error occurred while generating the token");
            }
        }
    }
}
