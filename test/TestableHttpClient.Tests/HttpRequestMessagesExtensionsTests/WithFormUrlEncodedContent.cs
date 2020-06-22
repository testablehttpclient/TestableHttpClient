using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithFormUrlEncodedContent
    {
#nullable disable
        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NulCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>()));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithNumberOfRequests_NulCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>(), 1));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithFormUrlEncodedContent(null));

            Assert.Equal("nameValueCollection", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithFormUrlEncodedContent(null, 1));

            Assert.Equal("nameValueCollection", exception.ParamName);
            sut.Verify(x => x.With(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
        }
#nullable restore

        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithFormUrlEncodedContent(new Dictionary<string, string> { ["username"] = "alice" });

            sut.Verify(x => x.With(Its.AnyPredicate(), null, "form url encoded content 'username=alice'"));
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithNumberOfRequests_CallsWithCorrectly()
        {
            var sut = new Mock<IHttpRequestMessagesCheck>();

            sut.Object.WithFormUrlEncodedContent(new Dictionary<string, string> { ["username"] = "alice" }, 1);

            sut.Verify(x => x.With(Its.AnyPredicate(), (int?)1, "form url encoded content 'username=alice'"));
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage
            {
                Content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>())
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>());

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>())
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent(new Dictionary<string, string> { ["username"] = "alice" }));

            Assert.Equal("Expected at least one request to be made with form url encoded content 'username=alice', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["username"] = "alice" })
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("plain/text");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent(new Dictionary<string, string> { ["username"] = "alice" }));

            Assert.Equal("Expected at least one request to be made with form url encoded content 'username=alice', but no requests were made.", exception.Message);
        }
    }
}
