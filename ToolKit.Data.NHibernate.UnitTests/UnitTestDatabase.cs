using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using FluentNHibernate.Cfg.Db;
using ToolKit.Validation;

namespace ToolKit.Data.NHibernate.UnitTests
{
    /// <summary>
    /// An Implementation of A Database Handler Class Using NHibernate for UnitTests.
    /// </summary>
    public class UnitTestDatabase : NHibernateDatabaseBase
    {
        private static readonly object _lock = new object();

        private static bool _databaseCreated;

        private readonly string _callingClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestDatabase" /> class.
        /// </summary>
        /// <param name="assemblyContainingMappings">The assembly containing mappings.</param>
        /// <param name="initialization">The initialization function for database.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public UnitTestDatabase(Assembly assemblyContainingMappings, Action initialization)
            : base(assemblyContainingMappings)
        {
            UnitTests = true;
            _instance = this;

            _callingClass = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;

            InitializeDatabase(initialization);
        }

        /// <summary>
        /// Resets the switch that tells the Initialization function that the database was created.
        /// </summary>
        public static void ResetDatabaseCreated()
        {
            _databaseCreated = false;
        }

        /// <summary>
        /// Resets the Session Factory cache.
        /// </summary>
        public static void ResetInstanceCache()
        {
            foreach (var cacheKey in Cache.Select(k => k.Key).ToList())
            {
                _ = Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// This method will Initializes the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        public override void InitializeDatabase(Action initialization)
        {
            initialization = Check.NotNull(initialization, nameof(initialization));

            Monitor.Enter(_lock);

            try
            {
                // SchemaExport doesn't recreate the file/schema on subsequent initializations.
                if (!_databaseCreated && File.Exists($"{_callingClass}.db"))
                {
                    File.Delete($"{_callingClass}.db");
                }

                initialization();
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        /// <summary>
        /// Contains the details about the NHibernate Configuration
        /// </summary>
        /// <returns>the NHibernate Configuration</returns>
        protected override IPersistenceConfigurer DatabaseConfigurer()
        {
            _databaseCreated = true;
            return SQLiteConfiguration.Standard.UsingFile($"{_callingClass}.db");
        }
    }
}
