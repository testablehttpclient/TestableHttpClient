using System;
using System.Net.Http;

using Xunit;

namespace HttpClient.TestHelpers.Tests
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
    }
}
