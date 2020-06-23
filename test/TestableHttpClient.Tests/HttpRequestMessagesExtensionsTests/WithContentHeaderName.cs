using System;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContentHeaderName
    {
#nullable disable
        [Fact]
        public void WithContentHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithContentHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeader_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeader_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader(headerName, 1));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithContentHeader_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContentHeader("Content-Type");

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "content header 'Content-Type'"));
        }

        [Fact]
        public void WithContentHeader_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContentHeader("Content-Type", 1);

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "content header 'Content-Type'"));
        }
    }
}
