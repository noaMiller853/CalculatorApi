namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// A concrete implementation of the Strategy Pattern - performs division of two numbers.
    /// </summary>
    public class DivideOperation : IOperation
    {
        /// <summary>
        /// Executes the division of two decimal numbers.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of the division (a / b).</returns>
        public decimal Execute(decimal a, decimal b) => a / b;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string Name => "Divide";
    }
}
