using LivenUserAPI.DTOs;
using LivenUserAPI.Infrastructure.Security;
using LivenUserAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LivenUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                _logger.LogWarning("Login attempt with null data.");
                return BadRequest(new { Message = "Login data is null." });
            }

            try
            {
                var user = await _userService.AuthenticateUser(loginDTO);

                if (user == null)
                {
                    _logger.LogWarning($"Invalid login attempt for email: {loginDTO.Email}");
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                var token = _jwtTokenService.GenerateToken(user);
                _logger.LogInformation($"User {user.Email} logged in successfully.");

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the login process.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
