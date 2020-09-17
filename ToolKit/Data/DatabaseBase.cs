using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.Data
{
    /// <summary>
    /// A Base Abstract Implementation of A Database Handler Class.
    /// </summary>
    public abstract class DatabaseBase : IDatabase
    {
        /// <summary>
        /// Allow derived classes direct access to the database instance.
        /// </summary>
        [SuppressMessage(
            "Critical Code Smell",
            "S2223:Non-constant static fields should not be visible",
            Justification = "This field is used in UnitTest to 'mock' the underlying DB")]
        [SuppressMessage(
            "Usage",
            "CA2211:Non-constant fields should not be visible",
            Justification = "This field is used in UnitTest to 'mock' the underlying DB")]
        [SuppressMessage(
            "StyleCop.CSharp.MaintainabilityRules",
            "SA1401:FieldsMustBePrivate",
            Justification = "This field is used in UnitTest to 'mock' the underlying DB")]
        protected static IDatabase _instance;

        /// <summary>
        /// Gets the database instance.
        /// </summary>
        public static IDatabase Instance => _instance;

        /// <summary>
        /// Gets or sets a value indicating whether Unit Test are being run.
        /// </summary>
        public static bool UnitTests { get; protected set; }

        /// <summary>
        /// This method will Initialize the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        public abstract void InitializeDatabase(Action initialization);
    }
}
