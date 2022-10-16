using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient;

/// <summary>
/// This class helps creating an <see cref="HttpResponseMessage"/> using a fluent interface.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The HttpResponseMessage is only created and passed to the consumer.")]
[Obsolete("Use ConfiguredResponse or a custom IResponse instead.")]
public sealed class HttpResponseMessageBuilder
{
    private readonly HttpResponseMessage httpResponseMessage;

    public HttpResponseMessageBuilder()
    {
        httpResponseMessage = new HttpResponseMessage
        {
            Content = new StringContent("")
        };
    }

    internal HttpResponseMessageBuilder(HttpResponseMessage httpResponseMessage)
    {
        this.httpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
    }

    /// <summary>
    /// Specifies the version of the response.
    /// </summary>
    /// <param name="httpVersion">The <see cref="HttpVersion"/> of the response.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithHttpVersion(Version httpVersion)
    {
        httpResponseMessage.Version = httpVersion;
        return this;
    }

    /// <summary>
    /// Specifies the status code of the response.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> of the response.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithHttpStatusCode(HttpStatusCode statusCode)
    {
        httpResponseMessage.StatusCode = statusCode;
        return this;
    }

    /// <summary>
    /// Configure request headers using a builder by directly accessing the <see cref="HttpResponseHeaders"/>.
    /// </summary>
    /// <param name="responseHeaderBuilder">The builder for configuring the response headers.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithResponseHeaders(Action<HttpResponseHeaders> responseHeaderBuilder)
    {
        if (responseHeaderBuilder == null)
        {
            throw new ArgumentNullException(nameof(responseHeaderBuilder));
        }

        responseHeaderBuilder(httpResponseMessage.Headers);
        return this;
    }

    /// <summary>
    /// Adds a request header to the response.
    /// </summary>
    /// <param name="header">The name of the header to add.</param>
    /// <param name="value">The value of the header to add.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithResponseHeader(string header, string value)
    {
        if (string.IsNullOrEmpty(header))
        {
            throw new ArgumentNullException(nameof(header));
        }

        httpResponseMessage.Headers.Add(header, value);
        return this;
    }

    /// <summary>
    /// Specifies the content of the response.
    /// </summary>
    /// <param name="content">The <see cref="HttpContent"/> of the response.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithContent(HttpContent content)
    {
        httpResponseMessage.Content = content;
        return this;
    }

    private const string DefaultStringContentMediaType = "text/plain";
    /// <summary>
    /// Specifies string content for the response.
    /// </summary>
    /// <param name="content">The content of the response.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithStringContent(string content)
    {
        return WithStringContent(content, null, DefaultStringContentMediaType);
    }

    /// <summary>
    /// Specifies string content for the response, with a specific encoding.
    /// </summary>
    /// <param name="content">The content of the response.</param>
    /// <param name="encoding">The encoding for this response, defaults to utf-8 when null is passed.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithStringContent(string content, Encoding? encoding)
    {
        return WithStringContent(content, encoding, DefaultStringContentMediaType);
    }

    /// <summary>
    /// Specifies string content for the response.
    /// </summary>
    /// <param name="content">The content of the response.</param>
    /// <param name="encoding">The encoding for this response, defaults to utf-8 when null is passed.</param>
    /// <param name="mediaType">The mediatype for this response, defaults to text/plain when null is passed.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithStringContent(string content, Encoding? encoding, string mediaType)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        return WithContent(new StringContent(content, encoding, mediaType ?? DefaultStringContentMediaType));
    }

    /// <summary>
    /// Specifies json content for the response.
    /// </summary>
    /// <param name="jsonObject">The json object that should be serialized.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithJsonContent(object? jsonObject)
    {
        return WithJsonContent(jsonObject, null);
    }

    /// <summary>
    /// Specifies json content for the response.
    /// </summary>
    /// <param name="jsonObject">The json object that should be serialized.</param>
    /// <param name="mediaType">The media type for this content, defaults to application/json when null is passed.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithJsonContent(object? jsonObject, string? mediaType)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(jsonObject);

        var content = new ByteArrayContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue(mediaType ?? "application/json");

        return WithContent(content);
    }

    /// <summary>
    /// Specifies the request message resulting in this response.
    /// </summary>
    /// <param name="requestMessage">The <see cref="HttpRequestMessage"/> resulting in this response.</param>
    /// <returns>The <see cref="HttpResponseMessageBuilder"/> for further building of the response.</returns>
    public HttpResponseMessageBuilder WithRequestMessage(HttpRequestMessage? requestMessage)
    {
        httpResponseMessage.RequestMessage = requestMessage;
        return this;
    }

    /// <summary>
    /// Builds and returns the HttpResponseMessage.
    /// </summary>
    /// <returns>The <see cref="HttpResponseMessage"/></returns>
    public HttpResponseMessage Build()
    {
        return httpResponseMessage;
    }
}
