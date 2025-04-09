using Xunit;
using Moq;
using CalculatorApi.Services;
using CalculatorApi.Models;
using CalculatorApi.Services.Operations;

namespace CalculatorApi.Tests.Services
{
    public class CalculatorServiceTests
    {
        private readonly Mock<ILogger<CalculatorService>> _loggerMock;
        private readonly Mock<IOperationFactory> _operationFactoryMock;
        private readonly CalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            // יצירת Mock עבור ILogger
            _loggerMock = new Mock<ILogger<CalculatorService>>();

            // יצירת Mock עבור IOperationFactory
            _operationFactoryMock = new Mock<IOperationFactory>();

            // יצירת מופע של CalculatorService עם ה-Mocks
            _calculatorService = new CalculatorService(_loggerMock.Object, _operationFactoryMock.Object);
        }

        [Fact]
        public async Task CalculateAsync_ValidRequest_ReturnsExtendedCalculationResponse()
        {
            // Arrange (הכנה)
            var request = new CalculationRequest { FirstNumber = 5, SecondNumber = 3 };
            var operationType = OperationType.add;
            var mockOperation = new Mock<IOperation>();
            mockOperation.Setup(op => op.Name).Returns("Add");
            mockOperation.Setup(op => op.Execute(5, 3)).Returns(8);

            _operationFactoryMock.Setup(factory => factory.GetOperation("Add")).Returns(mockOperation.Object);

            // Act (ביצוע הפעולה הנבדקת)
            var response = await _calculatorService.CalculateAsync(request, operationType);

            // Assert (בדיקה שהתוצאה צפויה)
            Assert.NotNull(response);
            Assert.Equal(8, response.Result);
            Assert.Equal("Add", response.Operation);
        }

        [Fact]
        public async Task CalculateAsync_NullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            CalculationRequest request = null;
            var operationType = OperationType.add;

            // Act & Assert (ביצוע הפעולה הנבדקת ובדיקה שהחריגה נזרקת)
            await Assert.ThrowsAsync<ArgumentNullException>(() => _calculatorService.CalculateAsync(request, operationType));
        }

        [Fact]
        public async Task CalculateAsync_NullFirstNumber_ThrowsArgumentNullException()
        {
            // Arrange
            var request = new CalculationRequest { FirstNumber = null, SecondNumber = 3 };
            var operationType = OperationType.add;
            var mockOperation = new Mock<IOperation>();
            _operationFactoryMock.Setup(factory => factory.GetOperation("Add")).Returns(mockOperation.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _calculatorService.CalculateAsync(request, operationType));
        }

        [Fact]
        public async Task CalculateAsync_NullSecondNumber_ThrowsArgumentNullException()
        {
            // Arrange
            var request = new CalculationRequest { FirstNumber = 5, SecondNumber = null };
            var operationType = OperationType.add;
            var mockOperation = new Mock<IOperation>();
            _operationFactoryMock.Setup(factory => factory.GetOperation("Add")).Returns(mockOperation.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _calculatorService.CalculateAsync(request, operationType));
        }

        [Fact]
        public async Task CalculateAsync_UnsupportedOperation_ThrowsArgumentException()
        {
            // Arrange
            var request = new CalculationRequest { FirstNumber = 5, SecondNumber = 3 };
            var operationType = OperationType.subtract; // פעולה שאינה מוגדרת ב-Factory Mock

            _operationFactoryMock.Setup(factory => factory.GetOperation("Subtract"))
                .Throws(new ArgumentException("Unsupported operation: subtract"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _calculatorService.CalculateAsync(request, operationType));
        }

        // בדיקות נוספות יכולות לכלול תרחישי שגיאה פנימיים בפעולות הספציפיות
    }
}
