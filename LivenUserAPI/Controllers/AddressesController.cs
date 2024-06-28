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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(IAddressService addressService, IUserService userService, ILogger<AddressesController> logger)
        {
            _addressService = addressService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("GetAddressesById")]
        public async Task<ActionResult> GetAddressesById([FromQuery] int id)
        {
            var address = await _addressService.GetAddressById(id);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost("CreateAddress")]
        public async Task<ActionResult> CreateAddress(AddressDTO addressDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var user = await _userService.GetUserById(userId);

                if (addressDto == null || user == null)
                {
                    return BadRequest(new { Message = "Invalid address data or user not found." });
                }

                var address = AddressMappings.ToDomain(addressDto);
                await _addressService.CreateAddress(address, userId);

                return Ok("Address created successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new address.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetAllAddressesByUserId")]
        public async Task<ActionResult> GetAllAddressesByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                var allAddressesByUser = await _addressService.GetAllAddressesByUserId(userId);

                return Ok(allAddressesByUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all addresses for user ID {userId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
        
        [HttpPut("UpdateAddress/{addressId}")]
        public async Task<ActionResult> UpdateAddress(AddressDTO addressDTO, int addressId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var existingAddress = await _addressService.VerifyAddressByUserId(addressId, userId);

                if (!existingAddress)
                {
                    return NotFound(new { Message = $"Address with ID {addressId} not found for current user." });
                }

                var address = AddressMappings.ToDomain(addressDTO);
                address.UserId = userId;
                address.Id = addressId;

                await _addressService.UpdateAddress(address);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating address with ID {addressId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("DeleteAddress/{id}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                var address = await _addressService.GetAddressById(id);

                if (address == null || address.UserId != userId)
                {
                    return BadRequest(new { Message = "Address not found or does not belong to the user." });
                }

                await _addressService.DeleteAddress(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting address with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
