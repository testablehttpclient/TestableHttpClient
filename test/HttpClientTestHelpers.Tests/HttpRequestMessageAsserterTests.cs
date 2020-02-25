using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HttpClientTestHelpers.Tests
{
    public class HttpRequestMessageAsserterTests
    {
        [Fact]
        public void Constructor_NullRequestList_ThrowsArgumentNullException()
        {
#nullable disable
            Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null));
#nullable restore
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithUriPattern_NullOrEmptyPattern_ThrowsArgumentNullException(string pattern)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithUriPattern(pattern));

            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithUriPattern_RequestWithMatchingUri_DoesNotThrowException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            sut.WithUriPattern("https://example.com/");
        }

        [Fact]
        public void WithUriPattern_RequestWithMatchingUriAndNegationTurnedOn_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) }, true);

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://example.com/"));
            Assert.Equal("Expected no requests to be made with uri pattern 'https://example.com/', but one request was made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithNotMatchingUri_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://test.org/"));
            Assert.Equal("Expected at least one request to be made with uri pattern 'https://test.org/', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithStarPatternAndNoRequests_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("*"));
            Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_NullHttpMethod_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

#nullable disable
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null));
#nullable restore

            Assert.Equal("httpMethod", exception.ParamName);
        }

        [Fact]
        public void WithHttpMethod_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_RequestsWithIncorrectHttpMethod_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com/")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_RequestsWithCorrectMethod_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            var result = sut.WithHttpMethod(HttpMethod.Get);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public void WithHttpVersion_NullHttpVersion_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

#nullable disable
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null));
#nullable restore

            Assert.Equal("httpVersion", exception.ParamName);
        }

        [Fact]
        public void WithHttpVersion_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11));

            Assert.Equal("Expected at least one request to be made with HTTP Version '1.1', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpVersion_RequestsWithIncorrectHttpVersion_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage { Version = HttpVersion.Version20 } });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11));

            Assert.Equal("Expected at least one request to be made with HTTP Version '1.1', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpVersion_RequestsWithCorrectVersion_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage { Version = HttpVersion.Version11 } });

            var result = sut.WithHttpVersion(HttpVersion.Version11);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithRequestHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithRequestHeader("api-version");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithRequestHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with request header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("no-api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "unknown");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithMatchinHeader_ReturnsHttpRequestMessageAssert()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithRequestHeader("api-version", "1.0");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContentHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage();
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContentHeader("Content-Type");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContentHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with content header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithMatchingHeader_ReturnsHttpRequestMessageAssert()
        {
            var request = new HttpRequestMessage();
            request.Content = new StringContent("", Encoding.UTF8, "application/json");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContentHeader("Content-Type", "application/json; charset=utf-8");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Host")]
        [InlineData("Content-Type")]
        public void WithHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter(string headerName)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "host";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithHeader(headerName);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "test";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Disposition", "inline"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Disposition' and value 'inline', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Content-Type", "application/json")]
        [InlineData("Host", "Test")]
        public void WithHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage(string name, string value)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "example";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader(name, value));

            Assert.Equal($"Expected at least one request to be made with header '{name}' and value '{value}', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Content-Type", "application/json; charset=utf-8")]
        [InlineData("Host", "Test")]
        public void WithHeaderNameAndValue_RequestWithMatchinHeader_ReturnsHttpRequestMessageAssert(string name, string value)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "Test";
            request.Content = new StringContent("", Encoding.UTF8, "application/json");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithHeader(name, value);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Fact]
        public void WithConten_NullPattern_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null));

            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContent_RequestWithNotMatchingContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent("some content"));

            Assert.Equal("Expected at least one request to be made with content 'some content', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContent("");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public void Times_ValueLessThan0_ThrowsArgumentException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentException>(() => sut.Times(-1));

            Assert.Equal("count", exception.ParamName);
        }

        [Fact]
        public void Times_NoRequestsAndCount1_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(1));

            Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void Times_NoRequestsAndCount2_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(2));

            Assert.Equal("Expected 2 requests to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void Times_NoRequestsAndCount0_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var result = sut.Times(0);
            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
