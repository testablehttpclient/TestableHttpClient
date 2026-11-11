#if !NET11_0_OR_GREATER
#pragma warning disable RS0016 // Add public types and members to the declared API

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.iunion?view=net-11.0
public interface IUnion
{
    /// <summary>
    /// Gets the value contained in the union, or <see langword="null"/> if the union has no value.
    /// </summary>
    public object? Value { get; }
}

[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unionattribute?view=net-11.0
public sealed class UnionAttribute : Attribute;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.isclosedtypeattribute?view=net-11.0
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class IsClosedTypeAttribute : Attribute
{
    private Type[] derivedTypes = Type.EmptyTypes;

    /// <summary>
    /// Gets or sets the derived types of the closed type.
    /// A <see langword="null"/> value is normalized to an empty array.
    /// </summary>
    public Type[] DerivedTypes
    {
        get => derivedTypes;
        set => derivedTypes = value ?? Type.EmptyTypes;
    }
}

#pragma warning restore RS0016 // Add public types and members to the declared API
#endif
