﻿using System.Globalization;

namespace TestableHttpClient.Utils;

internal sealed class UriPattern
{
    public static UriPattern Any { get; } = new UriPattern();

    public Value Scheme { get; init; } = Value.Any();
    public Value Host { get; init; } = Value.Any();
    public Value Port { get; init; } = Value.Any();
    public Value Path { get; init; } = Value.Any();
    public Value Query { get; init; } = Value.Any();

    public bool Matches(Uri requestUri, UriPatternMatchingOptions options) =>
        Scheme.Matches(requestUri.Scheme, options.SchemeCaseInsensitive) &&
        Host.Matches(requestUri.Host, options.HostCaseInsensitive) &&
        Port.Matches(requestUri.Port.ToString(CultureInfo.InvariantCulture), true) &&
        Path.Matches(requestUri.AbsolutePath, options.PathCaseInsensitive) &&
        Query.Matches(requestUri.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);
}

