namespace TestableHttpClient.Response
{
    public abstract class ResponseBase : IResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        protected virtual HttpContent? GetContent(HttpResponseContext context)
        {
            return null;
        }

        public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.HttpResponseMessage.StatusCode = StatusCode;
            context.HttpResponseMessage.Content = GetContent(context);
            return Task.CompletedTask;
        }
    }
}
