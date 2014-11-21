using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolKit
{
    /// <summary>
    /// Represents the abstract definition of an object which may be suspended. 
    /// </summary>
    public abstract class SuspendableObject
    {
        /// <summary>
        /// Gets a value indicating whether this instance is currently suspended.
        /// </summary>
        public bool Suspended
        {
            get
            {
                return IsSuspended();
            }
        }

        /// <summary>
        /// Determines whether this instance is currently suspended.
        /// </summary>
        /// <returns><c>true</c> if this instance is suspended; otherwise, <c>false</c>.</returns>
        public abstract bool IsSuspended();

        /// <summary>
        /// Resumes this instance.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Temporarily suspends this instance.
        /// </summary>
        public abstract void Suspend();
    }
}
