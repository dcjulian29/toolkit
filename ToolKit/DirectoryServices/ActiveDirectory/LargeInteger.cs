namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// Represents a 64-bit signed integer value.
    /// </summary>
    public class LargeInteger : IADsLargeInteger
    {
        /// <summary>
        /// Gets or sets the upper 32 bits of the integer.
        /// </summary>
        /// <value>The upper 32 bits of the integer.</value>
        public int HighPart { get; set; }

        /// <summary>
        /// Gets or sets the lower 32 bits of the integer.
        /// </summary>
        /// <value>The lower 32 bits of the integer.</value>
        public int LowPart { get; set; }
    }
}
