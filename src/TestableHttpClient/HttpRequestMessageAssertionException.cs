using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace TestableHttpClient
{
    [Serializable]
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not intended for public usage, but could be used for catching.")]
    public sealed class HttpRequestMessageAssertionException : Exception
    {
        internal HttpRequestMessageAssertionException(string message) : base(message)
        {
        }

        private HttpRequestMessageAssertionException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
