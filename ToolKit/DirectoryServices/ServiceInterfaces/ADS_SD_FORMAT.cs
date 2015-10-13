//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the format for converting the security descriptor.
    /// </summary>
    public enum ADS_SD_FORMAT
    {
        /// <summary>
        /// Indicates that the security descriptor is to be converted to the IADsSecurityDescriptor
        /// interface format. If ADS_SD_FORMAT_IID is used as the input format when setting the
        /// security descriptor, the variant passed in is expected to be a VT_DISPATCH, where the
        /// dispatch pointer supports the IADsSecurityDescriptor interface.
        /// </summary>
        IID = 1,

        /// <summary>
        /// Indicates that the security descriptor is to be converted to the binary format.
        /// </summary>
        RAW = 2,

        /// <summary>
        /// Indicates that the security descriptor is to be converted to the hex encoded string format.
        /// </summary>
        HEXSTRING = 3
    }
}
