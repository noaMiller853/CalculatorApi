namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// A concrete implementation of the Strategy Pattern - performs addition of two numbers.
    /// </summary>
    public class AddOperation : IOperation
    {
        /// <summary>
        /// Executes the addition of two decimal numbers.
        /// </summary>
        /// <param name="a">The first operand.</param>
        /// <param name="b">The second operand.</param>
        /// <returns>The sum of the two numbers.</returns>
        public decimal Execute(decimal a, decimal b) => a + b;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string Name => "Add";
    }
}
