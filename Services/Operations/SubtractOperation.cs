namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// A concrete implementation of the Strategy Pattern - performs subtraction of two numbers.
    /// </summary>
    public class SubtractOperation : IOperation
    {
        /// <summary>
        /// Executes the subtraction of two decimal numbers.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The difference (a - b).</returns>
        public decimal Execute(decimal a, decimal b) => a - b;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string Name => "Subtract";
    }
}
