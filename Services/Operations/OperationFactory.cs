using CalculatorApi.Models;

namespace CalculatorApi.Services.Operations
{
    /// <summary>
    /// Implements the Factory Pattern - responsible for creating and managing operation strategy instances.
    /// Singleton Pattern - registered as a singleton in the application to ensure a single shared instance.
    /// </summary>
    public class OperationFactory : IOperationFactory
    {
        private readonly IEnumerable<IOperation> _operations;

        /// <summary>
        /// Initializes the factory with a collection of available operation strategies.
        /// </summary>
        /// <param name="operations">All registered <see cref="IOperation"/> implementations.</param>
        public OperationFactory(IEnumerable<IOperation> operations)
        {
            _operations = operations;
        }

        /// <summary>
        /// Retrieves the appropriate operation strategy by name.
        /// </summary>
        /// <param name="operationName">The name of the desired operation (e.g., "Add", "Divide").</param>
        /// <returns>The matching <see cref="IOperation"/> implementation.</returns>
        /// <exception cref="ArgumentException">Thrown if the operation is not supported.</exception>
        public IOperation GetOperation(string operationName)
        {
            var operation = _operations.FirstOrDefault(op =>
                op.Name.Equals(operationName, StringComparison.OrdinalIgnoreCase));

            if (operation == null)
                throw new ArgumentException($"Unsupported operation: {operationName}");

            return operation;
        }
    }
}
