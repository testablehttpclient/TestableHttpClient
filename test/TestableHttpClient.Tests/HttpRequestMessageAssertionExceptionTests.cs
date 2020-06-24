using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

        [Fact]
        public void Serialization_Works()
        {
            var sut = new HttpRequestMessageAssertionException("My exception");

            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, sut);

            stream.Seek(0, SeekOrigin.Begin);

            var output = formatter.Deserialize(stream);

            var exception = Assert.IsType<HttpRequestMessageAssertionException>(output);
            Assert.Equal("My exception", exception.Message);
        }
    }
}
