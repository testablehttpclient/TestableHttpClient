namespace TestableHttpClient;

public interface IResponse
{
    Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken);
}
