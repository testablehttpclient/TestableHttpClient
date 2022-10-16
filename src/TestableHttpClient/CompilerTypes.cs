﻿// This file is used to register types that are normally generated by the compiler
// but aren't generated for older versions of .NET
#if !NET6_0_OR_GREATER
using System.ComponentModel;

namespace System.Runtime.CompilerServices;

// This class is used for init setters, which are introduced in .NET 6
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit { }
#endif
