namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides a placeholder required by the C# compiler to enable support
    /// for the 'init' accessor and 'record' types in environments where
    /// these features are not natively available (such as Unity).
    ///
    /// This type normally exists in .NET 5+.
    /// Unity projects using .NET Standard 2.1 must polyfill it manually.
    /// </summary>
    public sealed class IsExternalInit
    {
    }
}