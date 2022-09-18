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
                cancellationSourceField = typeof(CancellationToken).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Where(x => x.FieldType == typeof(CancellationTokenSource)).FirstOrDefault();
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