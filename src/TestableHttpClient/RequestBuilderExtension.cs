namespace TestableHttpClient;

public static class RequestBuilderExtension
{
    /// <summary>
    /// Configures the RequestBuilder to use the HTTP GET method and the specified request URI pattern.
    /// </summary>
    /// <param name="builder">The RequestBuilder to configure.</param>
    /// <param name="pattern">The request URI or URI template to apply to the builder.</param>
    /// <exception cref="System.ArgumentNullException">when the builder or pattern is null</exception>
    /// <returns>The configured RequestBuilder.</returns>
    public static RequestBuilder Get(this RequestBuilder builder, string pattern)
    {
        Guard.ThrowIfNull(builder);
        return builder.WithMethod(HttpMethod.Get).WithRequestUri(pattern);
    }

    /// <summary>
    /// Configures the RequestBuilder to use the HTTP POST method, the specified request URI pattern and content.
    /// </summary>
    /// <param name="builder">The RequestBuilder to configure.</param>
    /// <param name="pattern">The request URI or URI template to apply to the builder.</param>
    /// <param name="content">The expected content, supports wildcards.</param>
    /// <exception cref="System.ArgumentNullException">when the builder or pattern is null</exception>
    /// <returns>The configured RequestBuilder.</returns>
    public static RequestBuilder Post(this RequestBuilder builder, string pattern, string content)
    {
        Guard.ThrowIfNull(builder);
        return builder.WithMethod(HttpMethod.Post).WithRequestUri(pattern).WithContent(content);
    }

    /// <summary>
    /// Creates a POST request with the specified request URI pattern and JSON content.
    /// </summary>
    /// <param name="builder">The RequestBuilder to configure.</param>
    /// <param name="pattern">The request URI or route pattern.</param>
    /// <param name="content">The object to serialize to JSON for the request body, or null.</param>
    /// <exception cref="System.ArgumentNullException">when the builder or pattern is null</exception>
    /// <returns>The configured RequestBuilder.</returns>
    public static RequestBuilder PostAsJson(this RequestBuilder builder, string pattern, object? content)
    {
        Guard.ThrowIfNull(builder);

        return builder.WithMethod(HttpMethod.Post).WithRequestUri(pattern).WithJsonContent(content);
    }

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="builder">The implementation that hold all the request messages.</param>
    /// <param name="content">The object representation of the expected request content.</param>
    /// <returns>The <seealso cref="RequestBuilder"/> for further assertions.</returns>
    public static RequestBuilder WithJsonContent(this RequestBuilder builder, object? content) => builder.WithJsonContent(content, null);

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="builder">The implementation that hold all the request messages.</param>
    /// <param name="content">The object representation of the expected request content.</param>
    /// <param name="options">The serializer options that should be used for serializing te content.</param>
    /// <returns>The <seealso cref="RequestBuilder"/> for further assertions.</returns>
    public static RequestBuilder WithJsonContent(this RequestBuilder builder, object? content, JsonSerializerOptions? options)
    {
        Guard.ThrowIfNull(builder);

        string jsonContent = JsonSerializer.Serialize(content, options ?? builder.Options.JsonSerializerOptions);

        return builder.WithContent(jsonContent).WithHeader("Content-Type", "application/json*");
    }

    /// <summary>
    /// Asserts whether requests are made with specific url encoded content.
    /// </summary>
    /// <param name="builder">The implementation that hold all the request messages.</param>
    /// <param name="formValues">The collection of key/value pairs that should be url encoded.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static RequestBuilder WithFormUrlEncodedContent(this RequestBuilder builder, IEnumerable<KeyValuePair<string?, string?>> formValues)
    {
        Guard.ThrowIfNull(builder);

        using FormUrlEncodedContent content = new(formValues);
        string contentString = content.ReadAsStringAsync().Result;

        return builder.WithContent(contentString).WithHeader("Content-Type", "application/x-www-form-urlencoded*");
    }
}
