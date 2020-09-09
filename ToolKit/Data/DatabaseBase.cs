using System;

namespace ToolKit.Data
{
    /// <summary>
    /// A Base Abstract Implementation of A Database Handler Class
    /// </summary>
    public abstract class DatabaseBase : IDatabase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell",
            "S2223:Non-constant static fields should not be visible",
            Justification = "This field is used in UnitTest to 'mock' the underlying DB")]
        protected static IDatabase _instance;

        /// <summary>
        /// Gets or sets the database instance.
        /// </summary>
        public static IDatabase Instance => _instance;

        /// <summary>
        /// Gets or sets a value indicating whether Unit Test are being run.
        /// </summary>
        public static bool UnitTests { get; protected set; } = false;

        /// <summary>
        /// This method will Initialize the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        public abstract void InitializeDatabase(Action initialization);
    }
}
