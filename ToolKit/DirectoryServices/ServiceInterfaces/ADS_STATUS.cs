//// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the status of search preferences.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "This class contains references to Win32 API.")]
    public enum ADS_STATUS
    {
        /// <summary>
        /// The search preference was set successfully.
        /// </summary>
        S_OK = 0,

        /// <summary>
        /// The search preference specified in the dwSearchPref member of the ADS_SEARCHPREF_INFO
        /// structure is invalid. Search preferences must be taken from the ADS_SEARCHPREF_ENUM enumeration.
        /// </summary>
        INVALID_SEARCHPREF = 1,

        /// <summary>
        /// The value specified in the Value member of the ADS_SEARCHPREF_INFO structure is invalid
        /// for the corresponding search preference.
        /// </summary>
        INVALID_SEARCHPREFVALUE = 2
    }
}
