using CalculatorApi.Controllers;
using CalculatorApi.Services.Interfaces;
using CalculatorApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
namespace CalculatorApi.Tests.Controllers;
public class CalculatorControllerTests
{
    private readonly Mock<ICalculatorService> _calculatorServiceMock;
    private readonly Mock<ILogger<CalculatorController>> _loggerMock;
    private readonly CalculatorController _calculatorController;

    public CalculatorControllerTests()
    {
        _calculatorServiceMock = new Mock<ICalculatorService>();
        _loggerMock = new Mock<ILogger<CalculatorController>>();
        _calculatorController = new CalculatorController(_calculatorServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Calculate_ValidRequestAndOperation_ReturnsOkWithResult()
    {
        // Arrange
        var request = new CalculationRequest { FirstNumber = 5, SecondNumber = 3 };
        string xOperation = "add";
        var expectedResponse = new CalculationResponse { Result = 8 };
        _calculatorServiceMock.Setup(service => service.CalculateAsync(request, OperationType.add))
                              .ReturnsAsync(ExtendedCalculationResponse.Create(8, "Add"));

        // Act
        var result = await _calculatorController.Calculate(request, xOperation) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var actualResponse = result.Value as CalculationResponse;
        Assert.NotNull(actualResponse);
        Assert.Equal(expectedResponse.Result, actualResponse.Result);
    }

    [Fact]
    public async Task Calculate_InvalidOperation_ReturnsBadRequestWithError()
    {
        // Arrange
        var request = new CalculationRequest { FirstNumber = 5, SecondNumber = 3 };
        string xOperation = "invalid";

        // Act
        var result = await _calculatorController.Calculate(request, xOperation) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        // Fix for the invalid cast
        var errorResponse = result.Value as dynamic;
        
        // Assert the error property
        Assert.NotNull(errorResponse);
        Assert.StartsWith("Invalid value for X-Operation", errorResponse.error);
    }
}


