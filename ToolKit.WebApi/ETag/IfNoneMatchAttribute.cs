using System;

namespace ToolKit.WebApi.ETag
{
    /// <summary>
    ///   Capture the ETag If-None-Match HTTP Header and store it into the parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class IfNoneMatchAttribute : ETagMatchAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="IfNoneMatchAttribute" /> class.
        /// </summary>
        public IfNoneMatchAttribute()
            : base(ETagMatch.IfNoneMatch)
        {
        }
    }
}
