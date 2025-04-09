namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// A concrete implementation of the Strategy Pattern - performs multiplication of two numbers.
    /// </summary>
    public class MultiplyOperation : IOperation
    {
        /// <summary>
        /// Executes the multiplication of two decimal numbers.
        /// </summary>
        /// <param name="a">The first operand.</param>
        /// <param name="b">The second operand.</param>
        /// <returns>The product of the two numbers.</returns>
        public decimal Execute(decimal a, decimal b) => a * b;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string Name => "Multiply";
    }
}
