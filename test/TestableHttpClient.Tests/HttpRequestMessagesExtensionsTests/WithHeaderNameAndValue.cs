using System;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithHeaderNameAndValue
    {
#nullable disable
        [Fact]
        public void WithHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", "someValue"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", "someValue", 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHeader(headerName, "someValue", 1));

            Assert.Equal("headerName", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHeader("someHeader", headerValue, 1));

            Assert.Equal("headerValue", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithHeaderNameAndValue_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithHeader("someHeader", "someValue");

            sut.Verify(x => x.With(Its.AnyPredicate(), null, "header 'someHeader' and value 'someValue'"));
        }

        [Fact]
        public void WithHeaderNameAndValue_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithHeader("someHeader", "someValue", 1);

            sut.Verify(x => x.With(Its.AnyPredicate(), (int?)1, "header 'someHeader' and value 'someValue'"));
        }
    }
}
