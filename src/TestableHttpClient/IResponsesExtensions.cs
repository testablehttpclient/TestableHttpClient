using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient;

/// <summary>
/// Provides an interface for registering external methods that provide
/// custom <see cref="IResponse" /> instances.
/// </summary>
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Used for extending Responses class")]
public interface IResponsesExtensions { }
