using System;
using System.Net.Http;
using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class HttpRequestMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasRequestHeader_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("host"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasRequestHeader_NullHeaderName_ThrowsArgumentNullException(string headerName)
        {
            using var sut = new HttpRequestMessage();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader(headerName));
            Assert.Equal("headerName", exception.ParamName);
        }

        [Fact]
        public void HasRequestHeader_NullRequestNonNullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("host", "value"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasRequestHeader_NullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException(string headerName)
        {
            using var sut = new HttpRequestMessage();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader(headerName, "value"));
            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasRequestHeader_NonNullHeaderNameAndNullHeaderValue_ThrowsArgumentNullException(string headerValue)
        {
            using var sut = new HttpRequestMessage();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("Host", headerValue));
            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasRequestHeader_ExistingHeaderName_ReturnsTrue()
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "example.com";

            Assert.True(sut.HasRequestHeader("Host"));
        }

        [Theory]
        [InlineData("Host")]
        [InlineData("Content-Type")]
        public void HasRequestHeader_NotExistingHeaderName_ReturnsFalse(string headerName)
        {
            using var sut = new HttpRequestMessage();

            Assert.False(sut.HasRequestHeader(headerName));
        }

        [Theory]
        [InlineData("example.com")]
        [InlineData("example*")]
        [InlineData("*.com")]
        [InlineData("*")]
        public void HasRequestHeader_ExistingHeaderNameMatchingValue_ReturnsTrue(string value)
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "example.com";

            Assert.True(sut.HasRequestHeader("Host", value));
        }

        [Fact]
        public void HasRequestHeader_NotExitingHeaderNameAndValue_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage();

            Assert.False(sut.HasRequestHeader("Content-Type"));
        }

        [Theory]
        [InlineData("example.com")]
        [InlineData("example*")]
        [InlineData("*.com")]
        public void HasRequestHeader_ExistingHeaderNameNotMatchingValue_ReturnsFalse(string value)
        {
            using var sut = new HttpRequestMessage();
            sut.Headers.Host = "myhost.net";

            Assert.False(sut.HasRequestHeader("Host", value));
        }
    }
}
