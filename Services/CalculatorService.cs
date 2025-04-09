using CalculatorApi.Models;
using CalculatorApi.Services.Interfaces;
using CalculatorApi.Services.Operations;

namespace CalculatorApi.Services
{
    /// <summary>
    /// Implements the Service Layer Pattern - encapsulates business logic related to calculations.
    /// Acts as a Strategy Pattern Consumer - delegates calculation logic to different strategies based on the operation type.
    /// </summary>
    public class CalculatorService : ICalculatorService
    {
        private readonly ILogger<CalculatorService> _logger;
        private readonly IOperationFactory _operationFactory;

        /// <summary>
        /// Uses Constructor Injection Pattern - dependencies (logger and operation factory) are injected via the constructor.
        /// </summary>
        /// <param name="logger">Logger for logging calculation events and errors.</param>
        /// <param name="operatorFactory">Factory to retrieve the appropriate calculation strategy based on operation type.</param>
        public CalculatorService(ILogger<CalculatorService> logger, IOperationFactory operatorFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _operationFactory = operatorFactory ?? throw new ArgumentNullException(nameof(operatorFactory));
        }

        /// <summary>
        /// Uses the Template Method Pattern - defines the skeleton of the calculation process, while delegating specific steps
        /// (i.e., the actual arithmetic operation) to strategy implementations.
        /// </summary>
        /// <param name="request">The calculation request containing the operands.</param>
        /// <param name="operation">The type of mathematical operation to perform (e.g., Add, Subtract).</param>
        /// <returns>A response object containing the result and operation metadata.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the request or its values are null.</exception>
        public async Task<ExtendedCalculationResponse> CalculateAsync(CalculationRequest request, OperationType operation)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _logger.LogInformation("Calculating {Operation} for numbers {First} and {Second}",
                operation, request.FirstNumber, request.SecondNumber);

            try
            {
                // Retrieve the appropriate operation strategy (e.g., AddOperation, DivideOperation)
                IOperation operationStrategy = _operationFactory.GetOperation(operation.ToString());

                // Ensure both numbers are provided
                decimal firstNumber = request.FirstNumber ?? throw new ArgumentNullException(nameof(request.FirstNumber));
                decimal secondNumber = request.SecondNumber ?? throw new ArgumentNullException(nameof(request.SecondNumber));

                // Execute the operation
                decimal result = operationStrategy.Execute(firstNumber, secondNumber);

                // Wrap and return the result in a response object
                return ExtendedCalculationResponse.Create(result, operationStrategy.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during calculation");
                throw;
            }
        }
    }
}
