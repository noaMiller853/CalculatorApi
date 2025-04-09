namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// Interface defining the Strategy Pattern - provides a contract for calculation strategies.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Executes a specific arithmetic operation on two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The result of the operation.</returns>
        decimal Execute(decimal a, decimal b);

        /// <summary>
        /// Gets the name of the operation (e.g., "Add", "Divide").
        /// </summary>
        string Name { get; }
    }
}
