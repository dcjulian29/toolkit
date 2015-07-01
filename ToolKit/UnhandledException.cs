using System;
using System.Text;
using Common.Logging;

namespace ToolKit
{
    /// <summary>
    /// This class provides a hook and handle for unhandled exception. Call the Hook method when the
    /// application starts and then any unhandled exceptions will be logged.
    /// </summary>
    public class UnhandledException
    {
        private static ILog _log = LogManager.GetLogger<UnhandledException>();

        /// <summary>
        /// Prevents a default instance of the <see cref="UnhandledException"/> class from being created.
        /// </summary>
        private UnhandledException()
        {
        }

        /// <summary>
        /// Handles the unhandled exception by logging it to log4net.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">
        /// The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.
        /// </param>
        public static void Handle(object sender, UnhandledExceptionEventArgs e)
        {
            var builder = new StringBuilder();
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            builder.Append(Environment.NewLine);
            builder.Append("!!!!!!!!!!!!!!! An Unhandled Exception Has Occurred !!!!!!!!!!!!!!!");
            builder.Append(Environment.NewLine);
            builder.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            builder.Append(Environment.NewLine);
            builder.Append(e.ExceptionObject.ToString());
            builder.Append(Environment.NewLine);
            builder.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            _log.Fatal(builder.ToString());
        }

        /// <summary>
        /// Hooks the current application domain to add a callback to the "UnhandledException" event.
        /// </summary>
        public static void Hook()
        {
            AppDomain.CurrentDomain.UnhandledException += Handle;
        }
    }
}
