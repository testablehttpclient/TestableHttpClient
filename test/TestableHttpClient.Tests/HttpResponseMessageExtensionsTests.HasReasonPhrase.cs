using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class HttpResponseMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasReasonPhrase_WithReasonPhrase_NullResponse_ThrowsArgumentNullException()
        {
            HttpResponseMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasReasonPhrase("OK"));
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }

        [Fact]
        public void HasReasonPhrase_WithNullReasonPhrase_ThrowsArgumentNullException()
        {
            using var sut = new HttpResponseMessage { ReasonPhrase = "OK" };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasReasonPhrase(null));
            Assert.Equal("reasonPhrase", exception.ParamName);
        }
#nullable restore

        [Theory]
        [InlineData("")]
        [InlineData("OK")]
        public void HasReasonPhrase_WithCorrectReasonPhrase_ReturnsTrue(string reasonPhrase)
        {
            using var sut = new HttpResponseMessage { ReasonPhrase = reasonPhrase };

            Assert.True(sut.HasReasonPhrase(reasonPhrase));
        }

        [Theory]
        [InlineData("")]
        [InlineData("NotFound")]
        public void HasReasonPhrase_WithIncorrectReasonPhrase_ReturnsTrue(string reasonPhrase)
        {
            using var sut = new HttpResponseMessage { ReasonPhrase = "OK" };

            Assert.False(sut.HasReasonPhrase(reasonPhrase));
        }
    }
}
