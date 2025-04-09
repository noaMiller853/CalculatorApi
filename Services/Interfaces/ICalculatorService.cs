using CalculatorApi.Models;

namespace CalculatorApi.Services.Interfaces
{
    /// <summary>
    /// Interface Segregation Principle (ISP) - An interface that defines a single, specific responsibility.
    /// Contract Pattern - A clear contract for what the service should provide.
    /// </summary>
    public interface ICalculatorService
    {
        /// <summary>
        /// Performs a calculation operation asynchronously on two numbers.
        /// </summary>
        /// <param name="request">The calculation request containing two numeric values.</param>
        /// <param name="operation">The type of operation to perform (e.g., Add, Subtract, Multiply, Divide).</param>
        /// <returns>The result of the calculation, wrapped in an <see cref="ExtendedCalculationResponse"/> object.</returns>
        Task<ExtendedCalculationResponse> CalculateAsync(CalculationRequest request, OperationType operation);
    }
}
