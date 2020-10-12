using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the query preferences of the OLE DB for ADSI.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_PREFERENCES
    {
        /// <summary>
        /// Requests an asynchronous search.
        /// </summary>
        ASYNCHRONOUS = 0,

        /// <summary>
        /// Specifies that aliases of found objects are to be resolved. Use ADS_DEREFENUM to specify
        /// how to perform this operation.
        /// </summary>
        DEREF_ALIASES = 0x1,

        /// <summary>
        /// Specifies the size limit that the server should observe in a search. The size limit is
        /// the maximum number of returned objects. A zero value indicates that no size limit is
        /// imposed. The server stops searching once the size limit is reached and returns the
        /// results accumulated up to that point.
        /// </summary>
        SIZE_LIMIT = 0x2,

        /// <summary>
        /// Specifies the time limit, in seconds, that the server should observe in a search. A zero
        /// value indicates that no time limit restriction is imposed. When the time limit is
        /// reached, the server stops searching and returns results accumulated to that point.
        /// </summary>
        TIME_LIMIT = 0x3,

        /// <summary>
        /// Indicates that the search should obtain only the name of attributes to which values have
        /// been assigned.
        /// </summary>
        ATTRIBTYPES_ONLY = 0x4,

        /// <summary>
        /// Specifies the search scope that should be observed by the server. For more information
        /// about the appropriate settings, see the ADS_SCOPEENUM enumeration.
        /// </summary>
        SEARCH_SCOPE = 0x5,

        /// <summary>
        /// Specifies the time limit, in seconds, that a client will wait for the server to return
        /// the result.
        /// </summary>
        TIMEOUT = 0x6,

        /// <summary>
        /// Specifies the page size in a paged search. For each request by the client, the server
        /// returns, at most, the number of objects as set by the page size.
        /// </summary>
        PAGESIZE = 0x7,

        /// <summary>
        /// Specifies the time limit, in seconds, that the server should observe to search a page of
        /// results; this is opposed to the time limit for the entire search.
        /// </summary>
        PAGED_TIME_LIMIT = 0x8,

        /// <summary>
        /// Specifies that referrals may be chased. If the root search is not specified in the
        /// naming context of the server or when the search results cross a naming context (for
        /// example, when you have child domains and search in the parent domain), the server sends
        /// a referral message to the client which the client can choose to ignore or chase. By
        /// default, this option is set to ADS_CHASE_REFERRALS_EXTERNAL. For more information about
        /// referrals chasing, see ADS_CHASE_REFERRALS_ENUM.
        /// </summary>
        CHASE_REFERRALS = 0x9,

        /// <summary>
        /// Specifies that the server sorts the result set. Use the ADS_SORTKEY structure to specify
        /// the sort keys.
        /// </summary>
        SORT_ON = 0xa,

        /// <summary>
        /// Specifies if the result should be cached on the client side. By default, ADSI caches the
        /// result set. Turning off this option may be more desirable for large result sets.
        /// </summary>
        CACHE_RESULTS = 0xb,

        /// <summary>
        /// Allows the OLEDB client to specify bind flags to use when binding to the server. Valid
        /// values are those allowed by ADsOpenObject. It is accessed from ADO scripts using the
        /// property name "ADSI Flag."
        /// </summary>
        ADSIFLAG = 0xc
    }
}
