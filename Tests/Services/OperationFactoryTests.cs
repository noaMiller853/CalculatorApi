using Xunit;
using CalculatorApi.Services.Operations;
using Moq;
namespace CalculatorApi.Tests.Services
{
public class OperationFactoryTests
{
    [Fact]
    public void GetOperation_SupportedOperation_ReturnsCorrectOperation()
    {
        // Arrange
        var addOperationMock = new Mock<IOperation>();
        addOperationMock.Setup(op => op.Name).Returns("Add");
        var subtractOperationMock = new Mock<IOperation>();
        subtractOperationMock.Setup(op => op.Name).Returns("Subtract");

        var operations = new List<IOperation> { addOperationMock.Object, subtractOperationMock.Object };
        var factory = new OperationFactory(operations);

        // Act
        var operation = factory.GetOperation("Add");

        // Assert
        Assert.NotNull(operation);
        Assert.Equal("Add", operation.Name);
    }

    [Fact]
    public void GetOperation_UnsupportedOperation_ThrowsArgumentException()
    {
        // Arrange
        var addOperationMock = new Mock<IOperation>();
        addOperationMock.Setup(op => op.Name).Returns("Add");
        var operations = new List<IOperation> { addOperationMock.Object };
        var factory = new OperationFactory(operations);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => factory.GetOperation("Multiply"));
    }

        [Fact]
        public void GetOperation_CaseInsensitiveOperationName_ReturnsCorrectOperation()
        {
            // Arrange
            var addOperationMock = new Mock<IOperation>();
            addOperationMock.Setup(op => op.Name).Returns("Add");
            var operations = new List<IOperation> { addOperationMock.Object };
            var factory = new OperationFactory(operations);

            // Act
            var operation = factory.GetOperation("add"); // שימוש באותיות קטנות

            // Assert
            Assert.NotNull(operation);
            Assert.Equal("Add", operation.Name);
        }  }
}
