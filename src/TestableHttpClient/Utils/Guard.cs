using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TestableHttpClient.Utils;

internal static class Guard
{
    internal static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NETSTANDARD
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
#else
        ArgumentNullException.ThrowIfNull(argument, paramName);
#endif
    }

    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NETSTANDARD
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
        else if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentException("String should not be empty", paramName);
        }
#else
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#endif
    }
}
