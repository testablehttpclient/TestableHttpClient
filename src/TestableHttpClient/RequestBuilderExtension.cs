namespace TestableHttpClient;

public static class RequestBuilderExtension
{
    public static RequestBuilder Get(this RequestBuilder builder, string pattern)
    {
        Guard.ThrowIfNull(builder);
        return builder.WithMethod(HttpMethod.Get).WithRequestUri(pattern);
    }

    public static RequestBuilder Post(this RequestBuilder builder, string pattern, string content)
    {
        Guard.ThrowIfNull(builder);
        return builder.WithMethod(HttpMethod.Post).WithRequestUri(pattern).WithContent(content);
    }

    public static RequestBuilder PostAsJson(this RequestBuilder builder, string pattern, object? content)
    {
        Guard.ThrowIfNull(builder);

        return builder.WithMethod(HttpMethod.Post).WithRequestUri(pattern).WithJsonContent(content);
    }

    public static RequestBuilder WithJsonContent(this RequestBuilder builder, object? content) => builder.WithJsonContent(content, null);

    public static RequestBuilder WithJsonContent(this RequestBuilder builder, object? content, JsonSerializerOptions? options)
    {
        Guard.ThrowIfNull(builder);

        string jsonContent = JsonSerializer.Serialize(content, options ?? builder.Options.JsonSerializerOptions);

        return builder.WithContent(jsonContent).WithHeader("Content-Type", "application/json*");
    }

    public static RequestBuilder WithFormUrlEncodedContent(this RequestBuilder builder, IEnumerable<KeyValuePair<string?, string?>> formValues)
    {
        Guard.ThrowIfNull(builder);

        using FormUrlEncodedContent content = new(formValues);
        string contentString = content.ReadAsStringAsync().Result;

        return builder.WithContent(contentString).WithHeader("Content-Type", "application/x-www-form-urlencoded*");
    }
}
