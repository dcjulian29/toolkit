//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the available options that the IADsObjectOptions interface uses for manipulating
    /// directory objects.
    /// </summary>
    public enum ADS_OPTION
    {
        /// <summary>
        /// Gets a VT_BSTR that contains the host name of the server for the current binding to this
        /// object. This option is not supported by the IADsObjectOptions.SetOption method.
        /// </summary>
        SERVERNAME = 0,

        /// <summary>
        /// Gets or sets a VT_I4 value that indicates how referral chasing is performed in a query.
        /// This option can contain one of the values defined by the ADS_CHASE_REFERRALS_ENUM enumeration.
        /// </summary>
        REFERRALS = 1,

        /// <summary>
        /// Gets or sets a VT_I4 value that indicates the page size in a paged search.
        /// </summary>
        PAGE_SIZE = 2,

        /// <summary>
        /// Gets or sets a VT_I4 value that controls the security descriptor data that can be read
        /// on the object. This option can contain any combination of the values defined in the
        /// ADS_SECURITY_INFO_ENUM enumeration.
        /// </summary>
        SECURITY_MASK = 3,

        /// <summary>
        /// Gets a VT_I4 value that determines if mutual authentication is performed by the SSPI
        /// layer. If the returned option value contains the ISC_RET_MUTUAL_AUTH flag, defined in
        /// SSPI.h, then mutual authentication has been performed. If the returned option value does
        /// not contain the ISC_RET_MUTUAL_AUTH flag, then mutual authentication has not been
        /// performed. For more information about mutual authentication, see SSPI. This option is
        /// not supported by the IADsObjectOptions.SetOption method.
        /// </summary>
        MUTUAL_AUTH_STATUS = 4,

        /// <summary>
        /// Enables the effective quota and used quota of a security principal to be read. This
        /// option takes a VT_BSTR value that contains the security principal that the quotas can be
        /// read for. If the security principal string is zero length or the value is a VT_EMPTY
        /// value, the security principal is the currently logged on user. This option is only
        /// supported by the IADsObjectOptions.SetOption method. <blockquote>Windows XP, Windows
        /// 2000, and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        QUOTA = 5,

        /// <summary>
        /// Retrieves or sets a VT_I4 value that contains the port number that ADSI uses to
        /// establish a connection when the password is set or changed. By default, ADSI uses port
        /// 636 to establish a connection to set or change the password. <blockquote>Windows XP,
        /// Windows 2000, and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        PASSWORD_PORTNUMBER = 6,

        /// <summary>
        /// Retrieves or sets a VT_I4 value that specifies the password encoding method. This option
        /// can contain one of the values defined in the ADS_PASSWORD_ENCODING_ENUM enumeration.
        /// <blockquote>Windows XP, Windows 2000, and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        PASSWORD_METHOD = 7,

        /// <summary>
        /// Contains a VT_BOOL value that specifies if attribute value change operations should be
        /// accumulated. By default, when an attribute value is modified more than one time, the
        /// previous value change operation is overwritten by the more recent operation. If this
        /// option is set to VARIANT_TRUE, each attribute value change operation is accumulated in
        /// the cache. When the attribute value updates are committed to the server with the
        /// IADs.SetInfo method, each individual accumulated operation is sent to the server. When
        /// this option has been set to VARIANT_TRUE, it cannot be reset to VARIANT_FALSE for the
        /// lifetime of the ADSI object. To reset this option, all references to the ADSI object
        /// must be released and the object must be bound to again. When the object is bound to
        /// again, this option will be set to VARIANT_FALSE by default. This option only affects
        /// attribute values modified with the IADs PutEx and IADsPropertyList.PutPropertyItem
        /// methods. This option is ignored by the IADs.Put method. <blockquote>Windows Server 2003,
        /// Windows XP, Windows 2000, and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        ACCUMULATIVE_MODIFICATION = 8,

        /// <summary>
        /// If this option is set on the object, no lookups will be performed (either during the
        /// retrieval or during modification). This option affects the IADs and IADsPropertyList
        /// interfaces. It is also applicable when retrieving the effective quota usage of a
        /// particular user. <blockquote>Operating Systems prior to Windows Server 2008, Windows
        /// Vista, and Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        SKIP_SID_LOOKUP = 9
    }
}
