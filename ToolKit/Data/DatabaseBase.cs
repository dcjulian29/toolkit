namespace ToolKit.Data
{
    /// <summary>
    /// A Base Abstract Implementation of A Database Handler Class
    /// </summary>
    public abstract class DatabaseBase : IDatabase
    {
        protected static readonly IDatabase _instance;

        /// <summary>
        /// Gets the database instance.
        /// </summary>
        public static IDatabase Instance => _instance;

        /// <summary>
        /// Gets or sets a value indicating whether Unit Test are being run.
        /// </summary>
        public static bool UnitTests { get; protected set; } = false;

        /// <summary>
        /// This method will Initializes the database in a class that inherits from this base class.
        /// </summary>
        public virtual void InitializeDatabase()
        {
        }
    }
}
