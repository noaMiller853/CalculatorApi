namespace CalculatorApi.Models
{
    /// <summary>
    /// Represents a detailed calculation result, including metadata.
    /// 
    /// POCO (Plain Old CLR Object) Pattern - a simple data container without business logic.
    /// Immutable Object Pattern - properties are set only during creation to ensure immutability.
    /// </summary>
    public class ExtendedCalculationResponse : CalculationResponse
    {
        /// <summary>
        /// Private constructor to enforce immutability.
        /// </summary>
        private ExtendedCalculationResponse() { }

        /// <summary>
        /// Factory Method Pattern - creates a new immutable instance of <see cref="ExtendedCalculationResponse"/>.
        /// </summary>
        /// <param name="result">The result of the calculation.</param>
        /// <param name="operation">The name of the operation performed.</param>
        /// <returns>A fully populated <see cref="ExtendedCalculationResponse"/> instance.</returns>
        public static ExtendedCalculationResponse Create(decimal result, string operation)
        {
            return new ExtendedCalculationResponse
            {
                Result = result,
                Operation = operation,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
