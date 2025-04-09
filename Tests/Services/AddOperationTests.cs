using Xunit;
using CalculatorApi.Services.Operations;

namespace CalculatorApi.Tests.Services
{
    public class AddOperationTests
    {
        [Fact]
        public void Execute_PositiveNumbers_ReturnsSum()
        {
            // Arrange
            var addOperation = new AddOperation();
            decimal a = 5;
            decimal b = 3;

            // Act
            decimal result = addOperation.Execute(a, b);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void Execute_NegativeNumbers_ReturnsSum()
        {
            // Arrange
            var addOperation = new AddOperation();
            decimal a = -5;
            decimal b = -3;

            // Act
            decimal result = addOperation.Execute(a, b);

            // Assert
            Assert.Equal(-8, result);
        }

        [Fact]
        public void Execute_PositiveAndNegative_ReturnsSum()
        {
            // Arrange
            var addOperation = new AddOperation();
            decimal a = 5;
            decimal b = -3;

            // Act
            decimal result = addOperation.Execute(a, b);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Name_ReturnsCorrectName()
        {
            // Arrange
            var addOperation = new AddOperation();

            // Act
            string name = addOperation.Name;

            // Assert
            Assert.Equal("Add", name);
        }
    }
}
