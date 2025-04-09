namespace CalculatorApi.Authentication
{
    /// <summary>
    /// Options Pattern - holds the configuration settings for JWT generation and validation.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key used for signing the token.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Token expiration time in minutes.
        /// </summary>
        public int ExpirationMinutes { get; set; }

        /// <summary>
        /// The token issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The audience for whom the token is intended.
        /// </summary>
        public string Audience { get; set; }
    }
}
