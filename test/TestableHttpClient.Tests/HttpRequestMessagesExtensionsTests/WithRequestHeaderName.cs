using System;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithRequestHeaderName
    {
#nullable disable
        [Fact]
        public void WithRequestHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithRequestHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeader_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeader_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader(headerName, 1));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithRequestHeader_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithRequestHeader("api-version");

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "request header 'api-version'"));
        }

        [Fact]
        public void WithRequestHeader_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithRequestHeader("api-version", 1);

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "request header 'api-version'"));
        }
    }
}
