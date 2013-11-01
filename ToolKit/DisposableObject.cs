using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolKit
{
    /// <summary>
    /// An abstract class that wraps an object that uses scarce resources and will dispose
    /// those resource when they are no longer needed.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        #region -- Fields --
        private bool _disposed = false;
        #endregion
        #region -- Constructors (ctor) --
        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }
        #endregion
        #region -- Delegates --
        #endregion
        #region -- Events --
        #endregion
        #region -- Enumerations --
        #endregion
        #region -- Properties --
        #endregion
        #region -- Methods: Public --
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeResources(disposing);
                _disposed = true;
            }
        }
        #endregion
        #region -- Methods: Protected --

        /// <summary>
        /// Disposes the resources used by the inherited class.
        /// </summary>
        /// <param name="disposing">if set to <c>false</c>, this method has been called by the runtime.</param>
        protected abstract void DisposeResources(bool disposing);
        #endregion
        #region -- Methods: Native (WinAPI) --
        #endregion
        #region -- Methods: Private --
        #endregion
        #region -- Nested Types --
        #endregion
    }
}
