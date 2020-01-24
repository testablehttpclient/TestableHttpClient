using System;
using System.Net.Http;

using Xunit;

namespace HttpClientTestHelpers.Tests
{
    public partial class HttpRequestMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasHeader_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHeader("host"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasHeader_NullHeaderName_ThrowsArgumentNullException(string headerName)
        {
            using var sut = new HttpRequestMessage();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHeader(headerName));
            Assert.Equal("headerName", exception.ParamName);
        }

        [Fact]
        public void HasHeader_NullRequestNonNullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHeader("host", "value"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasHeader_NullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException(string headerName)
        {
            using var sut = new HttpRequestMessage();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHeader(headerName, "value"));
            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasHeader_NonNullHeaderNameAndNullHeaderValue_ThrowsArgumentNullException(string headerValue)
        {
            using var sut = new HttpRequestMessage();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHeader("Host", headerValue));
            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasHeader_ExistingHeaderName_ReturnsTrue()
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "example.com";

            Assert.True(sut.HasHeader("Host"));
        }

        [Fact]
        public void HasHeader_NotExistingHeaderName_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage();

            Assert.False(sut.HasHeader("Host"));
        }

        [Theory]
        [InlineData("example.com")]
        [InlineData("example*")]
        [InlineData("*.com")]
        [InlineData("*")]
        public void HasHeader_ExistingHeaderNameMatchingValue_ReturnsTrue(string value)
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "example.com";

            Assert.True(sut.HasHeader("Host", value));
        }

        [Theory]
        [InlineData("example.com")]
        [InlineData("example*")]
        [InlineData("*.com")]
        public void HasHeader_ExistingHeaderNameNotMatchingValue_ReturnsFalse(string value)
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "myhost.net";

            Assert.False(sut.HasHeader("Host", value));
        }
    }
}
