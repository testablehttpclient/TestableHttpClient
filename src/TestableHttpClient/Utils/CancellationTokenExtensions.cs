using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TestableHttpClient.Utils;

internal static class CancellationTokenExtensions
{
    private static FieldInfo? cancellationSourceField;

    [SuppressMessage("SonarSource", "S3011", Justification = "This reflection part is safe in our case. As long as a CancellationToken has just a single source.")]
    private static FieldInfo CancellationSourceField
    {
        get
        {
            if (cancellationSourceField is null)
            {
                cancellationSourceField =
                    typeof(CancellationToken)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Single(x => x.FieldType == typeof(CancellationTokenSource));
            }
            return cancellationSourceField;
        }
    }

    public static CancellationTokenSource? GetSource(this CancellationToken cancellationToken)
    {
        return CancellationSourceField.GetValue(cancellationToken) as CancellationTokenSource;
    }
}
