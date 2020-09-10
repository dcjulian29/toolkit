using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace ToolKit.WebApi.ETag
{
    /// <summary>
    ///   Reads the HTTP Request headers and assign it to the parameter.
    /// </summary>
    public class ETagParameterBinding : HttpParameterBinding
    {
        private readonly ETagMatch _match;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ETagParameterBinding" /> class.
        /// </summary>
        /// <param name="parameter">The parameter to assign.</param>
        /// <param name="match">Which header to match.</param>
        public ETagParameterBinding(HttpParameterDescriptor parameter, ETagMatch match)
                : base(parameter)
        {
            _match = match;
        }

        /// <summary>
        ///   Asynchronously executes the binding for the given request.
        /// </summary>
        /// <param name="metadataProvider">Metadata provider to use for validation.</param>
        /// <param name="actionContext">
        ///   The action context for the binding. The action context contains the parameter
        ///   dictionary that will get populated with the parameter.
        /// </param>
        /// <param name="cancellationToken">Cancellation token for canceling the binding operation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            EntityTagHeaderValue etagHeader = null;
            switch (_match)
            {
                case ETagMatch.IfNoneMatch:
                    etagHeader = actionContext.Request.Headers.IfNoneMatch.FirstOrDefault();
                    break;

                case ETagMatch.IfMatch:
                    etagHeader = actionContext.Request.Headers.IfMatch.FirstOrDefault();
                    break;
            }

            ETag etag = null;
            if (etagHeader != null)
            {
                etag = new ETag { Tag = etagHeader.Tag };
            }
            actionContext.ActionArguments[Descriptor.ParameterName] = etag;

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}
