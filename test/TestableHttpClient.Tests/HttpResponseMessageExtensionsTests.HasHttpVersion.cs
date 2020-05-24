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
        [Obsolete("Tests obsolete message", true)]
        public void HasHttpVersion_WithString_NullResponse_ThrowsArgumentNullException()
        {
            HttpResponseMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion("1.1"));
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpVersion_WithVersion_NullVersion_ThrowsArgumentNullException()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Unknown };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion((Version)null));
            Assert.Equal("httpVersion", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Obsolete("Tests obsolete message", true)]
        public void HasHttpVersion_WithString_NullVersion_ThrowsArgumentNullException(string httpVersion)
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Unknown };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(httpVersion));
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
        [Obsolete("Tests obsolete message", true)]
        public void HasHttpVersion_WithString_CorrectVersion_ReturnsTrue()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Version11 };

            Assert.True(sut.HasHttpVersion("1.1"));
        }

        [Fact]
        public void HasHttpVersion_WithVersion_IncorrectVersion_ReturnsFalse()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Version11 };

            Assert.False(sut.HasHttpVersion(HttpVersion.Version20));
        }

        [Fact]
        [Obsolete("Tests obsolete message", true)]
        public void HasHttpVersion_WithString_IncorrectVersion_ReturnsFalse()
        {
            using var sut = new HttpResponseMessage { Version = HttpVersion.Version11 };

            Assert.False(sut.HasHttpVersion("1.0"));
        }
    }
}
