using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.Tests.TestableHttpMessageHandlerResponseExtensionsTests
{
    public class SimulateTimeoutTests
    {
#nullable disable
        [Fact]
        public void SimulateTimeout_WhenHandlerIsNull_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.SimulateTimeout());
            Assert.Equal("handler", exception.ParamName);
        }
#nullable restore

        [Fact]
        public async Task SimulateTimout_WhenRequestIsMade_ThrowsTaskCancelationExceptionWithOperationCanceledMessage()
        {
            using var sut = new TestableHttpMessageHandler();
            sut.SimulateTimeout();
            using var client = new HttpClient(sut);

            var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => client.GetAsync(new Uri("https://example.com/")));
#if NET6_0_OR_GREATER
            Assert.Equal($"The request was canceled due to the configured HttpClient.Timeout of {client.Timeout.TotalSeconds} seconds elapsing.", exception.Message);
#else
            Assert.Equal(new OperationCanceledException().Message, exception.Message);
#endif
        }
    }
}
