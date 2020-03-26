using System;
using System.Net.Http;
using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class HttpRequestMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasHttpMethod_WithHttpMethod_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod(HttpMethod.Get));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpMethod_WithString_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod("GET"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpMethod_WithHttpMethod_NullHttpMethod_ThrowsArgumentNullException()
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod((HttpMethod)null));
            Assert.Equal("httpMethod", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasHttpMethod_WithString_NullHttpMethod_ThrowsArgumentNullException(string httpMethod)
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod(httpMethod));
            Assert.Equal("httpMethod", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasHttpMethod_WithHttpMethod_CorrectHttpMethod_ReturnsTrue()
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            Assert.True(sut.HasHttpMethod(HttpMethod.Get));
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("Get")]
        [InlineData("get")]
        public void HasHttpMethod_WithString_CorrectHttpMethod_ReturnsTrue(string httpMethod)
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            Assert.True(sut.HasHttpMethod(httpMethod));
        }

        [Fact]
        public void HasHttpMethod_WithHttpMethod_IncorrectHttpMethod_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            Assert.False(sut.HasHttpMethod(HttpMethod.Patch));
        }

        [Fact]
        public void HasHttpMethod_WithString_IncorrectHttpMethod_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage { Method = HttpMethod.Get };

            Assert.False(sut.HasHttpMethod("DELETE"));
        }
    }
}
