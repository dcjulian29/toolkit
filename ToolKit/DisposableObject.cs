using System;

namespace ToolKit
{
    /// <summary>
    /// An abstract class that wraps an object that uses scarce resources and will dispose those
    /// resource when they are no longer needed.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private bool _disposed = false;

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When requestin the runtime to not call the finalizer, this object is null.
        /// </exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeResources(disposing);
                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes the resources used by the inherited class.
        /// </summary>
        /// <param name="disposing">
        /// if set to <c>false</c>, this method has been called by the runtime.
        /// </param>
        protected abstract void DisposeResources(bool disposing);
    }
}
