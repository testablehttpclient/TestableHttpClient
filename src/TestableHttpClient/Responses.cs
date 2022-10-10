using TestableHttpClient.Response;

namespace TestableHttpClient;
public static class Responses
{
    public static IResponse Timeout() => new TimeoutResponse();
    public static IResponse Delayed(IResponse delayedResponse, int delayInMilliseconds) => new DelayedResponse(delayedResponse, delayInMilliseconds);
    public static IResponse Configured(IResponse response, Action<HttpResponseMessage> configureResponse) => new ConfiguredResponse(response, configureResponse);
    public static IResponse Sequenced(params IResponse[] responses) => new SequencedResponse(responses);
    public static IResponse StatusCode(HttpStatusCode statusCode) => new StatusCodeResponse(statusCode);
    public static IResponse NoContent() => StatusCode(HttpStatusCode.NoContent);
    public static IResponse Json(object content) => new JsonResponse(content);
}
