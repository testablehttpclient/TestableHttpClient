using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HttpClientTestHelpers.Tests
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
            Assert.Null(result.Content);
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
