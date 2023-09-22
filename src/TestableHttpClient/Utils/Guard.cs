using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TestableHttpClient.Utils;

internal static class Guard
{
    internal static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
#endif
    }

    internal static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET7_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrEmpty(argument, paramName);
#else

        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }
        else if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentException("String should not be empty", paramName);
        }
#endif
    }
}
