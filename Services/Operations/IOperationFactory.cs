namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// Factory Pattern - defines a contract for retrieving calculation strategies based on the operation name.
    /// </summary>
    public interface IOperationFactory
    {
        /// <summary>
        /// Retrieves an <see cref="IOperation"/> implementation based on the provided operation name.
        /// </summary>
        /// <param name="operationName">The name of the operation (e.g., "Add", "Multiply").</param>
        /// <returns>The corresponding <see cref="IOperation"/> instance.</returns>
        IOperation GetOperation(string operationName);
    }
}
