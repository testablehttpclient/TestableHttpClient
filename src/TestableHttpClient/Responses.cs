using TestableHttpClient.Response;

namespace TestableHttpClient;
public static class Responses
{
    public static IResponse SelectResponse(Func<HttpResponseContext, IResponse> selector) => new SelectableResponse(selector);
    public static IResponse Timeout() => new TimeoutResponse();
    public static IResponse Delayed(IResponse delayedResponse, int delayInMilliseconds) => new DelayedResponse(delayedResponse, delayInMilliseconds);
    public static IResponse Configured(IResponse response, Action<HttpResponseMessage> configureResponse) => new ConfiguredResponse(response, configureResponse);
    public static IResponse Sequenced(params IResponse[] responses) => new SequencedResponse(responses);
    public static IResponse StatusCode(HttpStatusCode statusCode) => new HttpResponse { StatusCode = statusCode };
    public static IResponse NoContent() => StatusCode(HttpStatusCode.NoContent);
    public static IResponse Text(string content, Encoding? encoding = null, string? mediaType = null) => new TextResponse(content, encoding, mediaType);
    public static IResponse Json(object? content, Encoding? encoding = null, string? mediaType = null) => new JsonResponse(content, encoding, mediaType);
    public static IResponse Json(object? content, HttpStatusCode statusCode, Encoding? encoding = null, string? mediaType = null) => new JsonResponse(content, encoding, mediaType) { StatusCode = statusCode };
    public static IResponsesExtensions Extensions { get; } = new ResponseExtensions();
    private sealed class ResponseExtensions : IResponsesExtensions { }
}
