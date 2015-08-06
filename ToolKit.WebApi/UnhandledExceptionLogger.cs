using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Common.Logging;

namespace ToolKit.WebApi
{
    /// <summary>
    /// Represents an unhandled exception logger that will log unhandled exceptions.
    /// </summary>
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private static ILog _log = LogManager.GetLogger<UnhandledExceptionLogger>();

        /// <summary>
        /// Adds an logger to the <see cref="HttpConfiguration"/> instance to log unhandled exceptions.
        /// </summary>
        /// <param name="config">An instance of the HttpConfiguration class.</param>
        public static void Add(HttpConfiguration config)
        {
            if (config != null)
            {
                config.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            var exception = context.Exception;

            _log.Fatal(m => m(exception.Message), exception);
        }
    }
}
