//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the type of initialization to be performed on a name translate object.
    /// </summary>
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
