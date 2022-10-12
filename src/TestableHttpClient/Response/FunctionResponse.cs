namespace TestableHttpClient.Response;

internal class FunctionResponse : ResponseBase
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage>? httpResponseMessageFactory;
    private readonly Action<HttpResponseMessageBuilder>? httpResponseMessageBuilderAction;

    internal FunctionResponse(Func<HttpRequestMessage, HttpResponseMessage> httpResponseMessageFactory)
    {
        this.httpResponseMessageFactory = httpResponseMessageFactory;
    }

    internal FunctionResponse(Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction)
    {
        this.httpResponseMessageBuilderAction = httpResponseMessageBuilderAction;
    }

    protected override HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
    {
        if (httpResponseMessageBuilderAction is not null)
        {
            var builder = new HttpResponseMessageBuilder();
            httpResponseMessageBuilderAction(builder);
            return builder.Build();
        }

        return httpResponseMessageFactory!(requestMessage);
    }
}
