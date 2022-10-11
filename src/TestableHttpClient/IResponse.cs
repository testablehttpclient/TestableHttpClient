namespace TestableHttpClient;

public interface IResponse
{
    Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage);
}
