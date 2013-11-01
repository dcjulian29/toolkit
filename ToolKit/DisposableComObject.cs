using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ToolKit
{
    /// <summary>
    /// An implementation class that can wrap a COM object that uses scarce resources
    /// and will dispose of those unmanaged resources when they are no longer needed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DisposableComObject : DisposableObject
    {
        #region -- Fields --
        #endregion
        #region -- Constructors (ctor) --
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableComObject"/> class.
        /// </summary>
        /// <param name="handle">A pointer to an external unmanaged resource.</param>
        public DisposableComObject(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableComObject"/> class.
        /// </summary>
        public DisposableComObject()
        {
            Handle = IntPtr.Zero;
        }
        #endregion
        #region -- Delegates --
        #endregion
        #region -- Events --
        #endregion
        #region -- Enumerations --
        #endregion
        #region -- Properties --
        /// <summary>
        /// Gets or sets the pointer to an external unmanaged resource.
        /// </summary>
        public IntPtr Handle
        {
            get;
            set;
        }
        #endregion
        #region -- Methods: Public --
        #endregion
        #region -- Methods: Protected --
        /// <summary>
        /// Disposes the resources used by the inherited class.
        /// </summary>
        /// <param name="disposing">if set to <c>false</c>, this method has been called by the runtime.</param>
        protected override void DisposeResources(bool disposing)
        {
            if (!disposing && Handle != IntPtr.Zero)
            {
                CloseHandle(Handle);
            }
        }
        #endregion
        #region -- Methods: Native (WinAPI) --
        [DllImport("Kernel32")]
        private static extern Boolean CloseHandle(IntPtr handle);
        #endregion
        #region -- Methods: Private --
        #endregion
        #region -- Nested Types --
        #endregion
    }
}
