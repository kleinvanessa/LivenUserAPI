using LivenUserAPI.Controllers;
using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;
using LivenUserAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LivenUserAPI.Tests.Controllers
{
    public class AddressesControllerTests
    {
        private readonly AddressesController _addressController;
        private MockRepository _mockRepository;
        private Mock<IAddressService> _mockAddressService;
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<AddressesController>> _mockLogger;

        public AddressesControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockAddressService = _mockRepository.Create<IAddressService>();
            _mockUserService = _mockRepository.Create<IUserService>();
            _mockLogger = new Mock<ILogger<AddressesController>>();

            _addressController = new AddressesController(
                _mockAddressService.Object,
                _mockUserService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreateAddress_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "Test", Email = "TestMail" };
            AddressDTO addressDto = new AddressDTO { City = "Test", Country = "TST" };

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync(user);

            _mockAddressService.Setup(s => s.CreateAddress(It.IsAny<Address>(), userId)).Returns(Task.CompletedTask);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.CreateAddress(addressDto) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Address created successfully!", result.Value.ToString());
        }

        [Fact]
        public async Task CreateAddress_ShouldReturn_BadRequest_WhenUserOrAddressDTOIsNull()
        {
            // Arrange
            var userId = 1;
            AddressDTO addressDto = null;

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync((User)null);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.CreateAddress(addressDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid address data or user not found.", result.Value.ToString());
        }

        [Fact]
        public async Task CreateAddress_ShouldReturn_Exception()
        {
            // Arrange
            var userId = 1;
            AddressDTO addressDto = null;

            _mockUserService.Setup(s => s.GetUserById(userId)).ThrowsAsync(new Exception("Create Address Error"));

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.CreateAddress(addressDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

        [Fact]
        public async Task GetAllAddressesByUserId_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "Test", Email = "TestMail" };
            List<Address> addresses = new List<Address>
            {
                new Address { City = "Test", Country = "TST" },
                new Address { City = "Sample", Country = "SMP" }
            };

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync(user);

            _mockAddressService.Setup(s => s.GetAllAddressesByUserId(userId)).ReturnsAsync(addresses);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.GetAllAddressesByUserId() as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(addresses.ToString(), result.Value.ToString());
        }

        [Fact]
        public async Task GetAllAddressesByUserId_ShouldReturn_NotFound()
        {
            // Arrange
            var userId = 1;

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync((User)null);


            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.GetAllAddressesByUserId() as ObjectResult;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", result.Value.ToString());
        }

        [Fact]
        public async Task GetAllAddressesByUserId_ShouldReturn_Exception()
        {
            // Arrange
            var userId = 1;

            _mockUserService.Setup(s => s.GetUserById(userId)).ThrowsAsync(new Exception("Error"));

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.GetAllAddressesByUserId() as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateAddress_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;
            AddressDTO addressDto = new AddressDTO { City = "Test", Country = "TST" };

            _mockAddressService.Setup(s => s.VerifyAddressByUserId(addressId, userId)).ReturnsAsync(true);

            _mockAddressService.Setup(s => s.UpdateAddress(It.IsAny<Address>())).Returns(Task.CompletedTask);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.UpdateAddress(addressDto, addressId) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Address updated successfully", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateAddress_ShouldReturn_NotFound()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;
            AddressDTO addressDto = new AddressDTO { City = "Test", Country = "TST" };

            _mockAddressService.Setup(s => s.VerifyAddressByUserId(addressId, userId)).ReturnsAsync(false);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.UpdateAddress(addressDto, addressId) as ObjectResult;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Address with ID {addressId} not found for current user.", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateAddress_ShouldReturn_Exception()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;
            AddressDTO addressDto = new AddressDTO { City = "Test", Country = "TST" };

            _mockAddressService.Setup(s => s.VerifyAddressByUserId(addressId, userId)).ThrowsAsync(new Exception("Error"));

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.UpdateAddress(addressDto, addressId) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

        [Fact]
        public async Task DeleteAddress_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;
            Address address = new Address { Id = addressId, City = "Test", Country = "TST", UserId = userId };


            _mockAddressService.Setup(s => s.GetAddressById(addressId)).ReturnsAsync(address);

            _mockAddressService.Setup(s => s.DeleteAddress(addressId)).Returns(Task.CompletedTask);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.DeleteAddress(addressId) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Address deleted successfully", result.Value.ToString());
        }

        [Fact]
        public async Task DeleteAddress_ShouldReturn_BadRequest_WhenAddressIsNull()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;
            Address address = null;


            _mockAddressService.Setup(s => s.GetAddressById(addressId)).ReturnsAsync(address);

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.DeleteAddress(addressId) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Address not found or does not belong to the user.", result.Value.ToString());
        }

        [Fact]
        public async Task DeleteAddress_ShouldReturn_Exception()
        {
            // Arrange
            var userId = 1;
            var addressId = 2;

            _mockAddressService.Setup(s => s.GetAddressById(addressId)).ThrowsAsync(new Exception("Error"));

            _addressController.ControllerContext.HttpContext = new DefaultHttpContext();
            _addressController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _addressController.DeleteAddress(addressId) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }
    }
}
