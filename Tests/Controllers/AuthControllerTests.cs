using Xunit;
using Moq;
using CalculatorApi.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<JwtTokenGenerator> _tokenGeneratorMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _tokenGeneratorMock = new Mock<JwtTokenGenerator>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_tokenGeneratorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetToken_ValidCall_ReturnsOkResultWithToken()
        {
            // Arrange
            string expectedToken = "generatedTestToken";
            var loginRequest = new LoginRequest { Username = "testUser", Password = "testPassword" };
            _tokenGeneratorMock.Setup(generator => generator.GenerateToken(It.IsAny<string>())).Returns(expectedToken);

            // Act
            var result = _authController.GetToken(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var tokenResult = result.Value as dynamic; // Use dynamic to access the token property
            Assert.NotNull(tokenResult);
            Assert.Equal(expectedToken, tokenResult.token);
        }

        [Fact]
        public void GetToken_TokenGenerationThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            _tokenGeneratorMock.Setup(generator => generator.GenerateToken(It.IsAny<string>()))
                .Throws(new Exception("Token generation failed"));

            // Act
            var loginRequest = new LoginRequest { Username = "testUser", Password = "testPassword" };
            var result = _authController.GetToken(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("An error occurred while generating the token", result.Value);
        }

        [Fact]
        public void GetToken_TokenGeneratorReturnsEmptyToken_ReturnsOkWithEmptyToken()
        {
            // Arrange
            string expectedToken = string.Empty;
            _tokenGeneratorMock.Setup(generator => generator.GenerateToken(It.IsAny<string>())).Returns(expectedToken);

            // Act
            var loginRequest = new LoginRequest { Username = "testUser", Password = "testPassword" };
            var result = _authController.GetToken(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var tokenResult = result.Value as dynamic; // Use dynamic to access the token property
            Assert.NotNull(tokenResult);
            Assert.Equal(expectedToken, tokenResult.token);
        }
    } 
}