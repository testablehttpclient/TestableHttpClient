using TestableHttpClient.Response;

namespace TestableHttpClient;

/// <summary>
/// Factory methods to create IResponses.
/// </summary>
public static class Responses
{
    /// <summary>
    /// Create a response that selects the actual response to run based on the context.
    /// </summary>
    /// <param name="selector">The selector to select the response based on the context.</param>
    /// <returns>A SelectableResponse.</returns>
    public static IResponse SelectResponse(Func<HttpResponseContext, IResponse> selector) => new SelectableResponse(selector);
    /// <summary>
    /// Create a response that simulates a timeout of the HttpClient.
    /// </summary>
    /// <returns>A TimeoutResponse.</returns>
    public static IResponse Timeout() => new TimeoutResponse();
    /// <summary>
    /// Create a response that delays the real response.
    /// </summary>
    /// <param name="delayedResponse">The response that should be delayed.</param>
    /// <param name="delayInMilliseconds">The number of milliseconds to delay the response.</param>
    /// <returns>A delayed response.</returns>
    public static IResponse Delayed(IResponse delayedResponse, int delayInMilliseconds) => Delayed(delayedResponse, TimeSpan.FromMilliseconds(delayInMilliseconds));
    /// <summary>
    /// Create a response that delays the real response.
    /// </summary>
    /// <param name="delayedResponse">The response that should be delayed.</param>
    /// <param name="delay">The time interval to delay the response.</param>
    /// <returns>A delayed response.</returns>
    public static IResponse Delayed(IResponse delayedResponse, TimeSpan delay) => new DelayedResponse(delayedResponse, delay);
    /// <summary>
    /// Create a response that configures the HttpResponseMessage.
    /// </summary>
    /// <param name="response">The basic response.</param>
    /// <param name="configureResponse">The action to use the HttpResponseMessage.</param>
    /// <returns>A ConfiguredResponse</returns>
    public static IResponse Configured(IResponse response, Action<HttpResponseMessage> configureResponse) => new ConfiguredResponse(response, configureResponse);
    /// <summary>
    /// Create a response that changes whith every call.
    /// </summary>
    /// <param name="responses">The response to return in order.</param>
    /// <returns>A sequenced response.</returns>
    public static IResponse Sequenced(params IResponse[] responses) => new SequencedResponse(responses);
    /// <summary>
    /// Create a response with a specific status code.
    /// </summary>
    /// <param name="statusCode">The statuscode to set on the HttpResponseMessage.</param>
    /// <returns>An HttpResponse with the configured StatusCode.</returns>
    public static IResponse StatusCode(HttpStatusCode statusCode) => new HttpResponse(statusCode);
    /// <summary>
    /// Create a response with some text content.
    /// </summary>
    /// <param name="content">The content to put in the response.</param>
    /// <param name="encoding">The encoding of the content, defaults to UTF-8.</param>
    /// <param name="mediaType">The media type of the content, defaults to 'text/plain'.</param>
    /// <returns>A response with specific content.</returns>
    public static IResponse Text(string content, Encoding? encoding = null, string? mediaType = null) => new TextResponse(content, encoding, mediaType);
    /// <summary>
    /// Create a response with json content.
    /// </summary>
    /// <param name="content">The content to serialize.</param>
    /// <param name="contentType">The content type of the response, defaults to 'application/json'.</param>
    /// <returns>A response with specific content.</returns>
    public static IResponse Json(object? content, string? contentType = null, JsonSerializerOptions? jsonSerializerOptions = null) => new JsonResponse(content, contentType) { JsonSerializerOptions = jsonSerializerOptions };
    /// <summary>
    /// Create a response with json content and a specific status code.
    /// </summary>
    /// <param name="content">The content to serialize.</param>
    /// <param name="statusCode">The status code for the response.</param>
    /// <param name="contentType">The content type of the response, defaults to 'application/json'.</param>
    /// <returns>A response with specific content.</returns>
    public static IResponse Json(object? content, HttpStatusCode statusCode, string? contentType = null, JsonSerializerOptions? jsonSerializerOptions = null) => new JsonResponse(content, contentType) { StatusCode = statusCode, JsonSerializerOptions = jsonSerializerOptions };
    /// <summary>
    /// Create a response for several routes.
    /// </summary>
    /// <param name="builder">The route builder that can be used to configure multiple routes.</param>
    /// <returns>A response with routing capabilities.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the builder paramater is null.</exception>
    public static IResponse Route(Action<IRoutingResponseBuilder> builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        RoutingResponseBuilder routingResponseBuilder = new();
        builder(routingResponseBuilder);
        return routingResponseBuilder.RoutingResponse;
    }
    /// <summary>
    /// Entrypoint for extensions.
    /// </summary>
    public static IResponsesExtensions Extensions { get; } = new ResponseExtensions();
    private sealed class ResponseExtensions : IResponsesExtensions { }
}
