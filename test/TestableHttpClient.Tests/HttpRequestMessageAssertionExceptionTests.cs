using Xunit;

namespace TestableHttpClient.Tests
{
    public class HttpRequestMessageAssertionExceptionTests
    {
        [Fact]
        public void Constructor_ByDefault_SetsMessage()
        {
            var exception = new HttpRequestMessageAssertionException("My exception");

            Assert.Equal("My exception", exception.Message);
        }
    }
}
