using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ToolKit.WebApi.ETag
{
    /// <summary>
    ///   Capture the ETag HTTP Header and store it into the parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ETagMatchAttribute : ParameterBindingAttribute
    {
        private readonly ETagMatch _match;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ETagMatchAttribute" /> class.
        /// </summary>
        /// <param name="match">Which header to match.</param>
        protected ETagMatchAttribute(ETagMatch match)
        {
            _match = match;
        }

        /// <summary>
        ///   Gets the parameter binding.
        /// </summary>
        /// <param name="parameter">The parameter description.</param>
        /// <returns>The parameter binding.</returns>
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(ETag))
            {
                return new ETagParameterBinding(parameter, _match);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}
