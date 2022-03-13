namespace TestableHttpClient.Utils;

internal static class MessageBuilder
{
    internal static string BuildMessage(int? expectedCount, int actualCount, IEnumerable<string> conditions)
    {
        var pass = expectedCount switch
        {
            null => actualCount > 0,
            _ => expectedCount == actualCount
        };

        var expectedMessage = expectedCount switch
        {
            null => "at least one request",
            0 => "no requests",
            1 => "one request",
            _ => $"{expectedCount} requests"
        };

        var expectedConditions = string.Empty;
        if (conditions.Any())
        {
            expectedConditions = $" with {string.Join(", ", conditions)}";
        }

        var actualMessage = actualCount switch
        {
            0 => "no requests were made",
            1 => "one request was made",
            _ => $"{actualCount} requests were made"
        };

        if (pass)
        {
            return $"Expected {expectedMessage} to be made{expectedConditions}, and {actualMessage}.";
        }
        else
        {
            return $"Expected {expectedMessage} to be made{expectedConditions}, but {actualMessage}.";
        }
    }
}
