using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace ToolKit.WebApi.Http
{
    /// <summary>
    /// Reads the HTTP Request body and assign it to the the string or byte[] parameter.
    /// </summary>
    public class RawBodyParameterBinding : HttpParameterBinding
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawBodyParameterBinding"/> class.
        /// </summary>
        /// <param name="descriptor">Represents the HTTP parameter descriptor.</param>
        public RawBodyParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this class will read the entity body.
        /// </summary>
        public override bool WillReadBody
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Asynchronously executes the binding for the given request.
        /// </summary>
        /// <param name="metadataProvider">Metadata provider to use for validation.</param>
        /// <param name="actionContext">
        /// The action context for the binding. The action context contains the parameter dictionary
        /// that will get populated with the parameter.
        /// </param>
        /// <param name="cancellationToken">Cancellation token for cancelling the binding operation.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public override Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var binding = actionContext?.ActionDescriptor.ActionBinding;

            if (actionContext.Request.Method == HttpMethod.Get)
            {
                var taskSource = new TaskCompletionSource<object>();
                taskSource.SetResult(null);
                return taskSource.Task;
            }

            var parameter = binding.ParameterBindings
                .First(x => x.GetType() == typeof(RawBodyParameterBinding));

            var type = parameter.Descriptor.ParameterType;

            if (type == typeof(string))
            {
                return actionContext.Request.Content.ReadAsStringAsync().ContinueWith(
                    (task) =>
                    {
                        SetValue(actionContext, task.Result);
                    },
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current);
            }

            if (type == typeof(byte[]))
            {
                return actionContext.Request.Content.ReadAsByteArrayAsync().ContinueWith(
                    (task) =>
                    {
                        SetValue(actionContext, task.Result);
                    },
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current);
            }

            throw new InvalidOperationException("Non-supported parameter type!");
        }
    }
}
