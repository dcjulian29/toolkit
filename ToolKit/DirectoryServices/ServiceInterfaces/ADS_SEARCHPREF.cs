//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies preferences of the search.
    /// </summary>
    public enum ADS_SEARCHPREF
    {
        /// <summary>
        /// Specifies that searches should be performed asynchronously. By default, searches are
        /// synchronous. In a synchronous search, the IDirectorySearch::GetFirstRow and
        /// IDirectorySearch::GetNextRow methods do not return until the server returns the entire
        /// result, or for a paged search, the entire page. An asynchronous search blocks until one
        /// row of the search results is available, or until the timeout interval specified by the
        /// ADS_SEARCHPREF_TIMEOUT search preference elapses.
        /// </summary>
        ASYNCHRONOUS = 0,

        /// <summary>
        /// Specifies that aliases of found objects are to be resolved. Use the ADS_DEREFENUM
        /// enumeration to specify how this is performed.
        /// </summary>
        DEREF_ALIASES = 1,

        /// <summary>
        /// Specifies the size limit that the server should observe during a search. The server
        /// stops searching when the size limit is reached and returns the results accumulated to
        /// that point. If this value is zero, the size limit is determined by the directory
        /// service. The default for this value is zero. If this value is greater than the size
        /// limit determined by the directory service, the directory service limit takes precedence.
        /// For Active Directory, the size limit specifies the maximum number of objects to be
        /// returned by the search. Also for Active Directory, the maximum number of objects
        /// returned by a search is 1000 objects.
        /// </summary>
        SIZE_LIMIT = 2,

        /// <summary>
        /// Specifies the number of seconds that the server waits for a search to complete. When the
        /// time limit is reached, the server stops searching and returns the results accumulated to
        /// that point. If this value is zero, the timeout period is infinite. The default for this
        /// value is 120 seconds.
        /// </summary>
        TIME_LIMIT = 3,

        /// <summary>
        /// Indicates that the search should obtain only the name of attributes to which values are assigned.
        /// </summary>
        ATTRIBTYPES_ONLY = 4,

        /// <summary>
        /// Specifies the search scope that should be observed by the server. For more information
        /// about the appropriate settings, see the ADS_SCOPEENUM enumeration.
        /// </summary>
        SEARCH_SCOPE = 5,

        /// <summary>
        /// Specifies the time limit, in seconds, that a client will wait for the server to return
        /// the result. This option is set in an ADS_SEARCHPREF_INFO structure.
        /// </summary>
        TIMEOUT = 6,

        /// <summary>
        /// Specifies the page size in a paged search. For each request by the client, the server
        /// returns, at most, the number of objects as set by the page size. When page size is set,
        /// it is unnecessary to set the size limit. If a size limit is set, then the value for page
        /// size must be less than the value for size limit. If the value for page size exceeds size
        /// limit, then for Windows 2000, the ERROR_DS_SIZELIMIT_EXCEEDED error is returned with no
        /// rows, and for Windows XP, the ERROR_DS_SIZELIMIT_EXCEEDED error is returned with the
        /// number of rows specified by size limit.
        /// </summary>
        PAGESIZE = 7,

        /// <summary>
        /// Specifies the number of seconds that the server should wait for a page of search
        /// results, as opposed to the time limit for the entire search. When the time limit is
        /// reached, the server stops searching and returns the results obtained up to that point,
        /// along with a cookie that contains the data about where to resume searching. If this
        /// value is zero, the page timeout period is infinite. The default value for this limit is
        /// 120 seconds.
        /// </summary>
        PAGED_TIME_LIMIT = 8,

        /// <summary>
        /// Specifies that referrals may be chased. If the root search is not specified in the
        /// naming context of the server or when the search results cross a naming context, for
        /// example, when you have child domains and search in the parent domain, the server sends a
        /// referral message to the client which the client can choose to ignore or chase. For more
        /// information about referral chasing, see ADS_CHASE_REFERRALS_ENUM.
        /// </summary>
        CHASE_REFERRALS = 9,

        /// <summary>
        /// Specifies that the server sorts the result set. Use the ADS_SORTKEY structure to specify
        /// the sort keys. This search preference works only for directory servers that support the
        /// LDAP control for server-side sorting. Active Directory supports the sort control, but it
        /// can impact server performance, particularly if the results set is large. Active
        /// Directory supports only a single sort key.
        /// </summary>
        SORT_ON = 10,

        /// <summary>
        /// Specifies if the result should be cached on the client side. By default, ADSI caches the
        /// result set. Disabling this option may be desirable for large result sets.
        /// </summary>
        CACHE_RESULTS = 11,

        /// <summary>
        /// Specifies a directory synchronization (DirSync) search, which returns all changes since
        /// a specified state. In the ADSVALUE structure, set the dwType member to
        /// ADS_PROV_SPECIFIC. The ProviderSpecific member is an ADS_PROV_SPECIFIC structure whose
        /// lpValue member specifies a cookie that indicates the state from which changes are
        /// retrieved. The first time you use the DirSync control, set the dwLength and lpValue
        /// members of the ADS_PROV_SPECIFIC structure to zero and NULL respectively. After reading
        /// the results set returned by the search until IDirectorySearch::GetNextRow returns
        /// S_ADS_NOMORE_ROWS, call IDirectorySearch::GetColumn to retrieve the ADS_DIRSYNC_COOKIE
        /// attribute which contains a cookie to use in the next DirSync search. For more
        /// information, see Polling for Changes Using the DirSync Control and
        /// LDAP_SERVER_DIRSYNC_OID. This flag cannot be combined with ADS_SEARCHPREF_PAGESIZE. The
        /// caller must have the SE_SYNC_AGENT_NAME privilege.
        /// </summary>
        DIRSYNC = 12,

        /// <summary>
        /// Specifies whether the search should also return deleted objects that match the search
        /// filter. When objects are deleted, Active Directory moves them to a "Deleted Objects"
        /// container. By default, deleted objects are not included in the search results. In the
        /// ADSVALUE structure, set the dwType member to ADSTYPE_BOOLEAN. To include deleted
        /// objects, set the Boolean member of the ADSVALUE structure to TRUE. Not all attributes
        /// are preserved when the object is deleted. You can retrieve the objectGUID and RDN
        /// attributes. The distinguishedName attribute is the DN of the object in the "Deleted
        /// Objects" container, not the previous DN. The isDeleted attribute is TRUE for a deleted
        /// object. For more information, see Retrieving Deleted Objects.
        /// </summary>
        TOMBSTONE = 13,

        /// <summary>
        /// Specifies that the search should use the LDAP virtual list view (VLV) control.
        /// ADS_SEARCHPREF_VLV can be used to access both string-type and offset-type VLV searches,
        /// by setting the appropriate fields. These two options cannot be used simultaneously
        /// because it is not possible to set the VLV control to request a result set that is both
        /// located at a specific offset and follows a particular value in the sort sequence. To
        /// perform a string search, set the lpszTarget field in ADS_VLV to the string to be
        /// searched for. To perform an offset type search, set the dwOffset field in ADS_VLV. If
        /// you use an offset search, you must set lpszTarget to NULL. ADS_SEARCHPREF_SORT_ON must
        /// be set to TRUE when using ADS_SEARCHPREF_VLV. The sort order of the search results
        /// determines the order used for the VLV search. If performing an offset-type search, the
        /// offset is used as an index into the sorted list. If performing a string-type search, the
        /// server attempts to return the first entry which is greater-than-or-equal-to the string,
        /// based on the sort order. Caching of search results is disabled when ADS_SEARCHPREF_VLV
        /// is specified. If you assign ADS_SEARCHPREF_CACHE_RESULTS a TRUE, value when using
        /// ADS_SEARCHPREF_VLV, SetSearchPreference will fail and return the error
        /// E_ADS_BAD_PARAMETER. <blockquote>Windows 2000 and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        VLV = 14,

        /// <summary>
        /// Specifies that an attribute-scoped query search should be performed. The search is
        /// performed against those objects named in a specified attribute of the base object. The
        /// vValue member of the ADS_SEARCHPREF_INFO structure contains a ADSTYPE_CASE_IGNORE_STRING
        /// value which contains the lDAPDisplayName of attribute to search. This attribute must be
        /// a ADS_DN_STRING attribute. Only one attribute may be specified. Search scope is
        /// automatically set to ADS_SCOPE_BASE when using this preference, and attempting to set
        /// the scope otherwise will fail with the error E_ADS_BAD_PARAMETER. With the exception of
        /// the ADS_SEARCHPREF_VLV preference, all other preferences that use LDAP controls, such as
        /// ADS_SEARCHPREF_DIRSYNC, ADS_SEARCHPREF_TOMBSTONE, and so on, are not allowed when this
        /// preference is specified. <blockquote>Windows 2000 and Active Directory Client Extension:
        /// Not supported.</blockquote>
        /// </summary>
        ATTRIBUTE_QUERY = 15,

        /// <summary>
        /// Specifies that the search should return security access data for the specified
        /// attributes. The vValue member of the ADS_SEARCHPREF_INFO structure contains an
        /// ADS_INTEGER value that is a combination of one or more of the following values.
        /// <table><tr><th>Value</th><th>Description</th></tr><tr><td>ADS_SECURITY_INFO_OWNER</td><td>Reads
        /// the owner data.</td></tr><tr><td>ADS_SECURITY_INFO_GROUP</td><td>Reads the group
        /// data.</td></tr><tr><td>ADS_SECURITY_INFO_DACL</td><td>Reads the discretionary
        /// access-control list (DACL).</td></tr><tr><td>ADS_SECURITY_INFO_SACL</td><td>Reads the
        /// system access-control list (SACL).</td></tr></table> If you read a security descriptor
        /// without explicitly specifying a security mask using ADS_SEARCHPREF_SECURITY_MASK, it
        /// defaults to the equivalent of ADS_SECURITY_INFO_OWNER | ADS_SECURITY_INFO_GROUP |
        /// ADS_SECURITY_INFO_DACL. <blockquote>Windows 2000 and Active Directory Client Extension:
        /// Not supported.</blockquote>
        /// </summary>
        SECURITY_MASK = 16,

        /// <summary>
        /// Contains optional flags for use with the ADS_SEARCHPREF_DIRSYNC search preference. The
        /// vValue member of the ADS_SEARCHPREF_INFO structure contains an ADSTYPE_INTEGER value
        /// that is zero or a combination of one or more of the following values. For more
        /// information about the DirSync control, see Polling for Changes Using the DirSync Control
        /// and LDAP_SERVER_DIRSYNC_OID.
        /// <table><tr><th>Identifier</th><th>Value</th><th>Description</th></tr><tr><td>LDAP_DIRSYNC_OBJECT_SECURITY</td><td>1</td><td>If
        /// this flag is not present, the caller must have the replicate changes right. If this flag
        /// is present, the caller requires no rights, but is only allowed to see objects and
        /// attributes which are accessible to the caller. This value is not supported by domain
        /// controllers that are running Windows 2000
        /// Server.</td></tr><tr><td>LDAP_DIRSYNC_ANCESTORS_FIRST_ORDER</td><td>2048
        /// (0x00000800)</td><td>Return parent objects before child objects, when parent objects
        /// would otherwise appear later in the replication
        /// stream.</td></tr><tr><td>LDAP_DIRSYNC_PUBLIC_DATA_ONLY</td><td>8192
        /// (0x00002000)</td><td>Do not return private data in the search
        /// results.</td></tr><tr><td>LDAP_DIRSYNC_INCREMENTAL_VALUES</td><td>2147483648
        /// (0x80000000)</td><td>If this flag is not present, all of the values, up to a
        /// server-specified limit, in a multi-valued attribute are returned when any value changes.
        /// If this flag is present, only the changed values are returned. This value is not
        /// supported by domain controllers that are running Windows 2000
        /// Server.</td></tr></table><blockquote>Windows XP, Windows 2000 and Active Directory
        /// Client Extension: Not supported.</blockquote>
        /// </summary>
        DIRSYNC_FLAG = 17,

        /// <summary>
        /// The search should return distinguished names in Active Directory extended format. The
        /// vValue member of the ADS_SEARCHPREF_INFO structure contains an ADSTYPE_INTEGER value
        /// that contains zero if the GUID and SID portions of the DN string should be in hex format
        /// or one if the GUID and SID portions of the DN string should be in standard format. For
        /// more information about extended distinguished names, see LDAP_SERVER_EXTENDED_DN_OID.
        /// <blockquote>Windows XP, Windows 2000 and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        EXTENDED_DN = 18
    }
}
