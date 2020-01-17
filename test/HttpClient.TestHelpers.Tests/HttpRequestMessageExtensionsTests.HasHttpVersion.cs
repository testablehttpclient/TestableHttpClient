using System;
using System.Net;
using System.Net.Http;

using Xunit;

namespace HttpClient.TestHelpers.Tests
{
    public partial class HttpRequestMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasHttpVersion_WithVersion_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(HttpVersion.Version20));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpVersion_WithString_NullRequest_ThrowsArgumentNullException()
        {
            HttpRequestMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion("1.1"));
            Assert.Equal("httpRequestMessage", exception.ParamName);
        }

        [Fact]
        public void HasHttpVersion_WithVersion_NullVersion_ThrowsArgumentNullException()
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Unknown };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion((Version)null));
            Assert.Equal("httpVersion", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HasHttpVersion_WithString_NullVersion_ThrowsArgumentNullException(string httpVersion)
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Unknown };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(httpVersion));
            Assert.Equal("httpVersion", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasHttpVersion_WithVersion_CorrectVersion_ReturnsTrue()
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Version11 };

            Assert.True(sut.HasHttpVersion(HttpVersion.Version11));
        }

        [Fact]
        public void HasHttpVersion_WithString_CorrectVersion_ReturnsTrue()
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Version11 };

            Assert.True(sut.HasHttpVersion("1.1"));
        }

        [Fact]
        public void HasHttpVersion_WithVersion_IncorrectVersion_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Version11 };

            Assert.False(sut.HasHttpVersion(HttpVersion.Version20));
        }

        [Fact]
        public void HasHttpVersion_WithString_IncorrectVersion_ReturnsFalse()
        {
            using var sut = new HttpRequestMessage { Version = HttpVersion.Version11 };

            Assert.False(sut.HasHttpVersion("1.0"));
        }
    }
}
