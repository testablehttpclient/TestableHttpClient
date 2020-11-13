using System;
using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient
{
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not intended for public usage, but could be used for catching.")]
    public sealed class HttpRequestMessageAssertionException : Exception
    {
        internal HttpRequestMessageAssertionException(string message) : base(message)
        {
        }
    }
}
