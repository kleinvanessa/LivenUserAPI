using LivenUserAPI.Controllers;
using LivenUserAPI.Domain.Entities;
using LivenUserAPI.DTOs;
using LivenUserAPI.Infrastructure.Security;
using LivenUserAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LivenUserAPI.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private MockRepository _mockRepository;
        private Mock<IUserService> _mockUserService;
        private Mock<IJwtTokenService> _mockJwtTokenService;
        private Mock<ILogger<AuthController>> _mockLogger;

        public AuthControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockUserService = _mockRepository.Create<IUserService>();
            _mockJwtTokenService = _mockRepository.Create<IJwtTokenService>();
            _mockLogger = new Mock<ILogger<AuthController>>();

            _authController = new AuthController(_mockUserService.Object,_mockJwtTokenService.Object,_mockLogger.Object);
        }

        [Fact]
        public async Task Login_ShouldReturn_Success()
        {
            // Arrange
            var userId = 1;
            var token = "TokenTest";
            var user = new User { Id = userId, Name = "Test", Email = "TestMail", Password = "PassTest" };
            LoginDTO loginDTO = new LoginDTO { Email = user.Email, Password = user.Password };

            _mockUserService.Setup(s => s.AuthenticateUser(loginDTO)).ReturnsAsync(user);

            _mockJwtTokenService.Setup(s => s.GenerateToken(user)).Returns(token);

            // Act
            var result = await _authController.Login(loginDTO) as ObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturn_BadRequest_WhenUserDtoIsNull()
        {
            // Arrange
            LoginDTO loginDTO = null;

            // Act
            var result = await _authController.Login(loginDTO) as ObjectResult;

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Login data is null.", result.Value.ToString());
        }

        [Fact]
        public async Task Login_ShouldReturn_Unauthorized()
        {
            // Arrange
            LoginDTO loginDTO = new LoginDTO { Email = "TestMail", Password = "PassTest" };

            _mockUserService.Setup(s => s.AuthenticateUser(loginDTO)).ReturnsAsync((User)null);

            // Act
            var result = await _authController.Login(loginDTO) as ObjectResult;

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password.", result.Value.ToString());            
        }

        [Fact]
        public async Task Login_ShouldReturn_Exception()
        {
            // Arrange
            LoginDTO loginDTO = new LoginDTO { Email = "TestMail", Password = "PassTest" };

            _mockUserService.Setup(s => s.AuthenticateUser(loginDTO)).ThrowsAsync(new Exception("Login Error"));

            // Act
            var result = await _authController.Login(loginDTO) as ObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while processing your request.", result.Value.ToString());
        }
    }
}
