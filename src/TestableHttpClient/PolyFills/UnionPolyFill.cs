#if !NET11_0_OR_GREATER
#pragma warning disable RS0016 // Add public types and members to the declared API

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.iunion?view=net-11.0
public interface IUnion
{
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
    public Type[] DerivedTypes
    {
        get;
        set => field = value ?? Type.EmptyTypes;
    } = Type.EmptyTypes;
}

#pragma warning restore RS0016 // Add public types and members to the declared API
#endif
