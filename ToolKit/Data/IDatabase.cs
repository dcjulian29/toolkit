using System;

namespace ToolKit.Data
{
    /// <summary>
    /// A implementation of a Database handler class.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// This method will Initialize the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        void InitializeDatabase(Action initialization);
    }
}
