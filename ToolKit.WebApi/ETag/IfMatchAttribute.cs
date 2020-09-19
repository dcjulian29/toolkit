using System;

namespace ToolKit.WebApi.ETag
{
    /// <summary>
    /// Capture the If-Match ETag HTTP Header and store it into the parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class IfMatchAttribute : ETagMatchAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="IfMatchAttribute" /> class.
        /// </summary>
        public IfMatchAttribute()
            : base(ETagMatch.IfMatch)
        {
        }
    }
}
