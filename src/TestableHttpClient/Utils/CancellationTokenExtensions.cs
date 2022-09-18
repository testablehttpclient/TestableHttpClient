using System.Reflection;

namespace TestableHttpClient.Utils;

internal static class CancellationTokenExtensions
{
    private static FieldInfo? cancellationSourceField;

    private static FieldInfo? CancellationSourceField
    {
        get
        {
            if (cancellationSourceField is null)
            {
                cancellationSourceField = typeof(CancellationToken).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly).FirstOrDefault(x => x.FieldType == typeof(CancellationTokenSource));
            }
            return cancellationSourceField;
        }
    }

    public static CancellationTokenSource? GetSource(this CancellationToken cancellationToken)
    {
        if (CancellationSourceField is not null)
        {
            return CancellationSourceField.GetValue(cancellationToken) as CancellationTokenSource;
        }
        return null;
    }
}
