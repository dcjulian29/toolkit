using System;
using ToolKit.Data;
using ToolKit.Validation;

namespace Toolkit.Data.EFCore
{
    /// <summary>
    /// An implementation of A Database Handler Class Using Entity Framework Core.
    /// </summary>
    public class EFCoreDatabaseBase : DatabaseBase
    {
        /// <summary>
        /// Gets the database instance.
        /// </summary>
        public static new EFCoreDatabaseBase Instance => _instance as EFCoreDatabaseBase;

        /// <summary>
        /// This method will Initializes the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        public override void InitializeDatabase(Action initialization)
        {
            initialization = Check.NotNull(initialization, nameof(initialization));

            initialization();
        }
    }
}
