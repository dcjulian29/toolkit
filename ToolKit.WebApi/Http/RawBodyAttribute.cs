using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ToolKit.WebApi.Http
{
    /// <summary>
    /// Capture the entire content body and store it into the parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class RawBodyAttribute : ParameterBindingAttribute
    {
        /// <summary>
        /// Gets the parameter binding.
        /// </summary>
        /// <param name="parameter">The parameter description.</param>
        /// <returns>The parameter binding.</returns>
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException("Invalid Parameter!");
            }

            return new RawBodyParameterBinding(parameter);
        }
    }
}
