using Xunit;
using Microsoft.Extensions.Options;
using CalculatorApi.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace CalculatorApi.Tests.Services
{

public class JwtTokenGeneratorTests
    {
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly JwtTokenGenerator _tokenGenerator;
        private const string TestSecretKey = "super-secret-key-here-1234567890";
        private const int TestExpirationMinutes = 30;
        private const string TestIssuer = "TestIssuer";
        private const string TestAudience = "TestAudience";

        public JwtTokenGeneratorTests()
        {
            _jwtSettings = Options.Create(new JwtSettings
            {
                Secret = TestSecretKey,
                ExpirationMinutes = TestExpirationMinutes,
                Issuer = TestIssuer,
                Audience = TestAudience
            });
            _tokenGenerator = new JwtTokenGenerator(_jwtSettings);
        }

        [Fact]
        public void GenerateToken_ValidUserId_ReturnsNonEmptyToken()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            // Act
            string token = _tokenGenerator.GenerateToken(userId);

            // Assert
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateToken_ValidUserId_TokenContainsCorrectSubject()
        {
            // Arrange
            string userId = "testUser";

            // Act
            string token = _tokenGenerator.GenerateToken(userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.Equal(userId, jwtToken.Subject);
        }

        [Fact]
        public void GenerateToken_ValidUserId_TokenHasCorrectIssuerAndAudience()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            // Act
            string token = _tokenGenerator.GenerateToken(userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.Equal(TestIssuer, jwtToken.Issuer);
            Assert.Equal(TestAudience, jwtToken.Audiences.FirstOrDefault());
        }

        // בדיקות נוספות יכולות לכלול בדיקת תוקף הטוקן (expiration)
    }
}

