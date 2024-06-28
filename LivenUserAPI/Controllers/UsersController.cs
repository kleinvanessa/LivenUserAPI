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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }
            var user = UserMappings.ToDomain(userDto);

            await _userService.CreateUser(user);

            return Ok();
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {            
            await _userService.UpdateUser(user);
            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser()
        {
            // Obter o ID do usuário autenticado
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _userService.DeleteUser(id);

            return Ok();
        }

    }
}
