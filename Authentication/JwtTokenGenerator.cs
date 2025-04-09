using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CalculatorApi.Authentication
{
    /// <summary>
    /// Builder Pattern - responsible for step-by-step construction of a JWT token.
    /// </summary>
    public class JwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Options Pattern - retrieves JWT configuration via dependency injection.
        /// </summary>
        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Generates a JWT token with user claims.
        /// </summary>
        /// <param name="userId">The user's identifier to embed in the token.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateToken(string userId)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var signingKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
