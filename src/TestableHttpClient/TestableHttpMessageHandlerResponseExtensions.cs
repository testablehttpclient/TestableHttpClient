using TestableHttpClient.Response;

namespace TestableHttpClient;

public static class TestableHttpMessageHandlerResponseExtensions
{
    /// <summary>
    /// Configure the <see cref="HttpResponseMessage"/> that should be returned for each request.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be configured.</param>
    /// <param name="httpResponseMessage">The response message to return.</param>
    /// <remarks>The response is actually the exact same response for every single request and will not be modified by TestableHttpMessageHandler.</remarks>
    [Obsolete("Returning an instance is deprecated. It is adviced to create a new instance for each request.")]
    public static void RespondWith(this TestableHttpMessageHandler handler, HttpResponseMessage httpResponseMessage)
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (httpResponseMessage is null)
        {
            throw new ArgumentNullException(nameof(httpResponseMessage));
        }

        handler.RespondWith(_ => httpResponseMessage);
    }

    /// <summary>
    /// Configure the <see cref="HttpResponseMessage"/> that should be returned using a <see cref="HttpResponseMessageBuilder"/>.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be configured.</param>
    /// <param name="httpResponseMessageBuilderAction">An action that calls methods on the <see cref="HttpResponseMessageBuilder"/>.</param>
    public static void RespondWith(this TestableHttpMessageHandler handler, Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction)
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        if (httpResponseMessageBuilderAction is null)
        {
            throw new ArgumentNullException(nameof(httpResponseMessageBuilderAction));
        }

        handler.RespondWith(new FunctionResponse(httpResponseMessageBuilderAction));
    }

    /// <summary>
    /// Simulate a timeout on the request by throwing a TaskCanceledException when a request is received.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be configured.</param>
    public static void SimulateTimeout(this TestableHttpMessageHandler handler)
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        handler.RespondWith(Responses.Timeout());
    }
}
