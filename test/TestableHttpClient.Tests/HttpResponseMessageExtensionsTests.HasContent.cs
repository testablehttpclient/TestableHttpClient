using System;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class HttpResponseMessageExtensionsTests
    {
#nullable disable
        [Fact]
        public void HasContent_NullResponse_ThrowsArgumentNullException()
        {
            HttpResponseMessage sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContent());
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void HasContent_NullContent_ReturnsFalse()
        {
            using var sut = new HttpResponseMessage();

            Assert.False(sut.HasContent());
        }

        [Fact]
        public void HasContent_EmptyContent_ReturnsFalse()
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("")
            };

            Assert.False(sut.HasContent());
        }

        [Fact]
        public void HasContent_NotEmptyContent_ReturnsTrue()
        {
            using var sut = new HttpResponseMessage
            {
                Content = new StringContent("Some Content")
            };

            Assert.True(sut.HasContent());
        }
    }
}
