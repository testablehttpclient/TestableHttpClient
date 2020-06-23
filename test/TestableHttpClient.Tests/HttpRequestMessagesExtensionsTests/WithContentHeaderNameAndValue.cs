using System;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContentHeaderNameAndValue
    {
#nullable disable
        [Fact]
        public void WihtContentHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WihtContentHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue", 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader(headerName, "someValue", 1));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContentHeader("someHeader", headerValue, 1));

            Assert.Equal("headerValue", exception.ParamName);
            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContentHeader("someHeader", "someValue");

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "content header 'someHeader' and value 'someValue'"));
        }

        [Fact]
        public void WithContentHeaderNameAndValue_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContentHeader("someHeader", "someValue", 1);

            sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "content header 'someHeader' and value 'someValue'"));
        }
    }
}
