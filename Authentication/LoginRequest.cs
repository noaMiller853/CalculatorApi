namespace CalculatorApi.Authentication
{
    /// <summary>
    /// Simple model representing login request data
    /// </summary>
    public class LoginRequest
    {
        /// <example>admin</example>
        public string Username { get; set; }

        /// <example>1234</example>
        public string Password { get; set; }
    }
}
