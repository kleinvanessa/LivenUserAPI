using LivenUserAPI.Controllers;
using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;
using LivenUserAPI.Mappings;
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
    public class UsersControllerTests
    {
        private readonly UsersController _usersController;
        private MockRepository _mockRepository;
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<UsersController>> _mockLogger;

        public UsersControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockUserService = _mockRepository.Create<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>(); //_mockRepository.Create<ILogger<UsersController>>()           

            _usersController = new UsersController(_mockUserService.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task GetUserData_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "Test", Email = "TestMail" };
            var userDto = new UserDTO { Name = "Test", Email = "TestMail" };

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync(user);

            _usersController.ControllerContext.HttpContext = new DefaultHttpContext();
            _usersController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _usersController.GetUserData() as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserDTO>(result.Value);
            Assert.Equal(userDto.ToString(), result.Value.ToString());
        }

        [Fact]
        public async Task GetUserData_ShouldReturn_NotFound()
        {
            // Arrange
            var userId = 1;

            _mockUserService.Setup(s => s.GetUserById(userId)).ReturnsAsync((User)null);

            _usersController.ControllerContext.HttpContext = new DefaultHttpContext();
            _usersController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _usersController.GetUserData() as ObjectResult;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", result.Value.ToString());
        }

        [Fact]
        public async Task GetUserData_ShouldReturn_Error()
        {
            // Arrange
            var userId = 1;
            var exceptionMessage = "Database error";

            _mockUserService.Setup(s => s.GetUserById(userId)).ThrowsAsync(new Exception(exceptionMessage));

            _usersController.ControllerContext.HttpContext = new DefaultHttpContext();
            _usersController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            //_mockLogger.Setup(logger => logger.LogError(
            //    It.IsAny<Exception>(),
            //    It.IsAny<string>()))
            //    .Verifiable();

            // Act
            var result = await _usersController.GetUserData() as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());

            //_mockLogger.Verify(logger => logger.LogError(
            //    It.Is<string>(message => message.Contains($"An error occurred while getting user with ID {userId}.")),
            //    It.Is<Exception>(ex => ex.Message == exceptionMessage)),
            //    Times.Once);


            //_mockLogger.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_Success()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Test User", Email = "testuser@example.com" };

            _mockUserService.Setup(s => s.CreateUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _usersController.CreateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("User created successfully!", result.Value);
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_BadRequest_WhenUserDtoIsNull()
        {
            // Arrange
            UserDTO userDto = null;

            // Act
            var result = await _usersController.CreateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Invalid user data.", result.Value.ToString());
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_Exception()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Test User", Email = "testuser@example.com" };
            var user = UserMappings.ToDomain(userDto);
            var exceptionMessage = "Database error";

            _mockUserService.Setup(s => s.CreateUser(user)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.CreateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateUser_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDTO { Name = "Test User", Email = "testuser@example.com" };

            _mockUserService.Setup(s => s.UpdateUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            _usersController.ControllerContext.HttpContext = new DefaultHttpContext();
            _usersController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _usersController.UpdateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("User updated successfully", result.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturn_BadRequest_WhenUserDtoIsNull()
        {
            // Arrange
            UserDTO userDto = null;

            // Act
            var result = await _usersController.UpdateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Invalid user data.", result.Value.ToString());
        }

        [Fact]
        public async Task UpdateUser_ShouldReturn_Exception()
        {
            // Arrange
            var userDto = new UserDTO { Name = "Test User", Email = "testuser@example.com" };
            var user = UserMappings.ToDomain(userDto);
            var exceptionMessage = "Database error";

            _mockUserService.Setup(s => s.UpdateUser(user)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.UpdateUser(userDto) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

        [Fact]
        public async Task DeleteUser_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;

            _mockUserService.Setup(s => s.DeleteUser(It.IsAny<int>())).Returns(Task.CompletedTask);

            _usersController.ControllerContext.HttpContext = new DefaultHttpContext();
            _usersController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _usersController.DeleteUser() as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("User deleted successfully", result.Value);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturn_Exception()
        {
            // Arrange            
            var exceptionMessage = "Database error";
            var userId = 1;

            _mockUserService.Setup(s => s.DeleteUser(userId)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _usersController.DeleteUser() as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }

    }
}
