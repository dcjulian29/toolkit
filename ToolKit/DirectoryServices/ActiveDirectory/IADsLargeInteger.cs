namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// The IADsLargeInteger interface is used to manipulate 64-bit integers of the LargeInteger type.
    /// </summary>
    public interface IADsLargeInteger
    {
        /// <summary>
        /// Gets or sets the upper 32 bits of the integer.
        /// </summary>
        /// <value>The upper 32 bits of the integer.</value>
        int HighPart { get; set; }

        /// <summary>
        /// Gets or sets the lower 32 bits of the integer.
        /// </summary>
        /// <value>The lower 32 bits of the integer.</value>
        int LowPart { get; set; }
    }
}
