using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestableHttpClient.Tests
{
    public class HttpResponseMessageBuilderTests
    {
        [Fact]
        public void Build_ReturnsEmptyHttpResponseMessage()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.Build();

            Assert.NotNull(result);
            Assert.Equal(HttpVersion.Version11, result.Version);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Headers);
            Assert.NotNull(result.Content);
            Assert.Null(result.RequestMessage);
        }

        [Fact]
        public void WithVersion_CreatesHttpResponseMessageWithCorrectVersion()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithVersion(HttpVersion.Version20).Build();

            Assert.Equal(HttpVersion.Version20, result.Version);
        }

        [Fact]
        public void WithStatusCode_CreatesHttpResponseMessageWithCorrectStatusCode()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStatusCode(HttpStatusCode.BadRequest).Build();

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

#nullable disable
        [Fact]
        public void WithHeaders_WhenPassingNull_ArgumentNullExceptionIsThrown()
        {
            var sut = new HttpResponseMessageBuilder();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeaders(null));
            Assert.Equal("responseHeaderBuilder", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHeaders_CreatesHttpResponseMessageWithCorrectHeaders()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithHeaders(x => x.Location = new Uri("https://example.com/")).Build();

            Assert.Equal("https://example.com/", result.Headers.Location.AbsoluteUri);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeader_NullOrEmptyName_ThrowsArgumentException(string headerName)
        {
            var sut = new HttpResponseMessageBuilder();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName, "value"));
            Assert.Equal("header", exception.ParamName);
        }

        [Fact]
        public void WithHeader_CreatesHttpResponseMessageWithHeaderAddedToTheList()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithHeader("Location", "https://example.com/").Build();

            Assert.Equal("https://example.com/", result.Headers.GetValues("Location").Single());
        }

        [Fact]
        public void WithContent_CreatesHttpResponseMessageWithContent()
        {
            var sut = new HttpResponseMessageBuilder();
            using var content = new StringContent(string.Empty);

            var result = sut.WithContent(content).Build();

            Assert.Same(content, result.Content);
        }

#nullable disable
        [Fact]
        public void WithStringContent_NullContent_ThrowsArgumentNullException()
        {
            var sut = new HttpResponseMessageBuilder();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithStringContent(null));
            Assert.Equal("content", exception.ParamName);
        }
#nullable restore

        [Fact]
        public async Task WithStringContent_CreatesHttpResponseMessageWithStringContent()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStringContent("My content").Build();

            Assert.Equal("My content", await result.Content.ReadAsStringAsync());
            Assert.Equal("text/plain", result.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-8", result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public void WithStringContent_WithNullEncoding_CreateHttpResponseMessageWithDefaultEncoding()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStringContent("", null).Build();

            Assert.Equal("text/plain", result.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-8", result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public void WithStringContent_WithEncoding_CreatesHttpResponseMessageWithContentTypeHeader()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStringContent("", Encoding.ASCII).Build();

            Assert.Equal("text/plain", result.Content.Headers.ContentType.MediaType);
            Assert.Equal("us-ascii", result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public void WithStringContent_WithNullMediaType_CreateHttpResponseMessageWithDefaultMediaType()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStringContent("", null, null).Build();

            Assert.Equal("text/plain", result.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-8", result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public void WithStringContent_WithMediaType_CreatesHttpResponseMessageWithContentTypeHeader()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithStringContent("", null, "application/json").Build();

            Assert.Equal("application/json", result.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-8", result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public async Task WithJsonContent_Null_CreatesHttpResponseMessageWithNullJsonAndDefaultContentType()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithJsonContent(null).Build();

            Assert.Equal("null", await result.Content.ReadAsStringAsync());
            Assert.Equal("application/json", result.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task WithJsonContent_ObjectAndNullMediaType_CreatesHttpResponseMessageWithJsonAndDefaultContentType()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithJsonContent(Array.Empty<object>(), null).Build();

            Assert.Equal("[]", await result.Content.ReadAsStringAsync());
            Assert.Equal("application/json", result.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task WithJsonContent_ObjectAnCustomMediaType_CreatesHttpResponseMessageWithJsonAndContentType()
        {
            var sut = new HttpResponseMessageBuilder();

            var result = sut.WithJsonContent(new { }, "text/json").Build();

            Assert.Equal("{}", await result.Content.ReadAsStringAsync());
            Assert.Equal("text/json", result.Content.Headers.ContentType.MediaType);
            Assert.Null(result.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public void WithRequestMessage_CreatesHttpResponseMessagaeWithRequestMessage()
        {
            var sut = new HttpResponseMessageBuilder();
            using var requestMessage = new HttpRequestMessage();

            var result = sut.WithRequestMessage(requestMessage).Build();

            Assert.Same(requestMessage, result.RequestMessage);
        }
    }
}
