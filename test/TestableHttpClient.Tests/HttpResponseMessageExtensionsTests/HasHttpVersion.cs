using System;
using System.Net;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class HttpResponseMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasHttpVersion_WithVersion_NullResponse_ThrowsArgumentNullException()
        {
            HttpResponseMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(HttpVersion.Version20));
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpVersion_WithVersion_NullVersion_ThrowsArgumentNullException()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Unknown };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(null));
            Assert.Equal("httpVersion", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasHttpVersion_WithVersion_CorrectVersion_ReturnsTrue()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Version11 };

            Assert.True(sut.HasHttpVersion(HttpVersion.Version11));
        }

        [Fact]
        public void HasHttpVersion_WithVersion_IncorrectVersion_ReturnsFalse()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Version11 };

            Assert.False(sut.HasHttpVersion(HttpVersion.Version20));
        }
    }
}
