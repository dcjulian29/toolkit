using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the type of initialization to be performed on a name translate object.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_NAME_INITTYPE
    {
        /// <summary>
        /// Initializes a NameTranslate object by setting the domain that the object binds to.
        /// </summary>
        DOMAIN = 1,

        /// <summary>
        /// Initializes a NameTranslate object by setting the server that the object binds to.
        /// </summary>
        SERVER = 2,

        /// <summary>
        /// Initializes a NameTranslate object by locating the global catalog that the object binds to.
        /// </summary>
        GC = 3
    }
}
