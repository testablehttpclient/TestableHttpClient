using System;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContent
    {
#nullable disable
        [Fact]
        public void WithContent_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent("*"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithContent_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent("*", 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithContent_WithoutNumberOfRequests_NullPattern_ThrowsArgumentNullException()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContent(null));

            Assert.Equal("pattern", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void WithContent_WithNumberOfRequests_NullPattern_ThrowsArgumentNullException()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContent(null, 1));

            Assert.Equal("pattern", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithContent_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContent("some content");

            sut.Verify(x => x.With(Its.AnyPredicate(), null, "content 'some content'"));
        }

        [Fact]
        public void WithContent_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithContent("some content", 1);

            sut.Verify(x => x.With(Its.AnyPredicate(), (int?)1, "content 'some content'"));
        }
    }
}
