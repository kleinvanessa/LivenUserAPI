using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;
using LivenUserAPI.Mappings;
using LivenUserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LivenUserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetUserData")]
        public async Task<ActionResult> GetUserData()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var userDto = UserMappings.ToDTO(user);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with ID {userId}.");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest( "Invalid user data.");
            }

            try
            {
                var user = UserMappings.ToDomain(userDto);
                await _userService.CreateUser(user);

                return Ok("User created successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = UserMappings.ToDomain(userDto);
                user.Id = userId;

                await _userService.UpdateUser(user);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID.");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userService.DeleteUser(userId);

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user.");
                return BadRequest("An error occurred while processing your request.");
            }
        }

    }
}
