using System;

namespace ToolKit.WebApi.ETag
{
    /// <summary>
    ///   The ETag or entity tag is one of several mechanisms that HTTP provides for Web cache
    ///   validation, which allows a client to make conditional requests.
    /// </summary>
    public class EntityTag
    {
        /// <summary>
        ///   Gets or sets the opaque identifier assigned by a Web server to a specific version of a resource.
        /// </summary>
        public string Tag { get; set; }
    }
}
