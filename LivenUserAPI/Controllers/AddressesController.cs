using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;
using LivenUserAPI.Mappings;
using LivenUserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LivenUserAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;

        public AddressesController(IAddressService addressService, IUserService userService)
        {
            _addressService = addressService;
            _userService = userService;
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

        [HttpPost("CreateAddress/{userId}")]
        public async Task<ActionResult> CreateAddress(AddressDTO addressDto, int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (addressDto == null || user == null)
            {
                return BadRequest();
            }
            var address = AddressMappings.ToDomain(addressDto);

            await _addressService.CreateAddress(address, userId);

            return Ok();
        }

        [HttpGet("GetAllAddressesByUserId/{userId}")]
        public async Task<ActionResult> GetAllAddressesByUserId(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            var allAddressesByUser = await _addressService.GetAllAddressesByUserId(userId);

            return Ok(allAddressesByUser);
        }
        
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult> UpdateAddress(Address address)
        {
            await _addressService.UpdateAddress(address);

            return Ok();
        }

        [HttpDelete("DeleteAddress/{id}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            await _addressService.DeleteAddress(id);

            return Ok();
        }
    }
}
