namespace ToolKit.WebApi.ETag
{
    /// <summary>
    ///   ETag Header to match.
    /// </summary>
    public enum ETagMatch
    {
        /// <summary>
        ///   If-Match HTTP Header.
        /// </summary>
        IfMatch,

        /// <summary>
        ///   If-None-Match HTTP Header.
        /// </summary>
        IfNoneMatch
    }
}
