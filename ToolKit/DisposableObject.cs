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
    }
}
