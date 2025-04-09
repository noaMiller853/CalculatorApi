using Xunit;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.Extensions.Logging;
using CalculatorApi.Middleware;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text;

namespace CalculatorApi.Tests.Services
{
public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private readonly ExceptionHandlingMiddleware _middleware;
        private readonly DefaultHttpContext _httpContext;

        public ExceptionHandlingMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _middleware = new ExceptionHandlingMiddleware(_nextMock.Object, _loggerMock.Object);
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream(); 
        }

        [Fact]
        public async Task InvokeAsync_NextDelegateDoesNotThrow_NextDelegateIsCalled()
        {
            // Arrange
            bool nextCalled = false;
            _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
                     .Callback(() => nextCalled = true)
                     .Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.True(nextCalled);
        }

        [Fact]
        public async Task InvokeAsync_ArgumentExceptionThrown_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var exception = new ArgumentException("Invalid argument");
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(_httpContext.Response.Body, Encoding.UTF8);
            var responseBody = await reader.ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);
            Assert.NotNull(errorResponse);
            Assert.Equal(exception.Message, errorResponse.error);
            Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.statusCode);
            _loggerMock.Verify(
                x => x.LogError(exception, "Unhandled exception"),
                Times.Once);
        }

        // Helper class כדי לפענח את תגובת השגיאה JSON
        private class ErrorResponse
        {
            public string error { get; set; }
            public int statusCode { get; set; }
        }
    }
}

