using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using ToolKit.Validation;

namespace ToolKit.Data.NHibernate.UnitTests
{
    /// <summary>
    /// An Implementation of A Database Handler Class Using NHibernate for UnitTests.
    /// </summary>
    public class UnitTestDatabase : NHibernateDatabaseBase
    {
        private static readonly object _lock = new object();

        private string _callingClass;

        private bool _databaseCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestDatabase" /> class.
        /// </summary>
        /// <param name="initialization">The initialization function for database.</param>
        /// <param name="sessionName">The name of the session used in this unit test.</param>
        public UnitTestDatabase(Action initialization, ref string sessionName)
            : base(Assembly.GetCallingAssembly())
        {
            Initialize(initialization, ref sessionName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestDatabase" /> class.
        /// </summary>
        /// <param name="assemblyContainingMappings">The assembly containing mappings.</param>
        /// <param name="initialization">The initialization function for database.</param>
        /// <param name="sessionName">The name of the session used in this unit test.</param>
        public UnitTestDatabase(Assembly assemblyContainingMappings, Action initialization, ref string sessionName)
            : base(assemblyContainingMappings)
        {
            Initialize(initialization, ref sessionName);
        }

        /// <summary>
        /// Gets the name of the database file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the open session for this instance.
        /// </summary>
        public ISession Session
        {
            get => NHibernateDatabaseBase.Instance.SessionFactory(SessionName).OpenSession();
        }

        /// <summary>
        /// Gets the name of the session for this instance.
        /// </summary>
        public string SessionName { get; private set; }

        /// <summary>
        /// Remove a session from the Session Factory cache.
        /// </summary>
        /// <param name="sessionName">The name of the session.</param>
        public static void RemoveInstance(string sessionName)
        {
            if (Cache.Contains(sessionName))
            {
                _ = Cache.Remove(sessionName);
            }
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
                if (!_databaseCreated && File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                initialization();
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        /// <summary>
        /// Resets the switch that tells the Initialization function that the database was created.
        /// </summary>
        public void ResetDatabaseCreated() => _databaseCreated = false;

        /// <summary>
        /// Contains the details about the NHibernate Configuration.
        /// </summary>
        /// <returns>the NHibernate Configuration.</returns>
        protected override IPersistenceConfigurer DatabaseConfigurer()
        {
            _databaseCreated = true;
            return SQLiteConfiguration.Standard.UsingFile(FileName);
        }

        private void Initialize(Action initialization, ref string sessionName)
        {
            UnitTests = true;
            _instance = this;
            _callingClass = new StackTrace().GetFrame(2).GetMethod().ReflectedType.Name;
            sessionName = SessionName = Guid.NewGuid().ToString();
            FileName = $"{_callingClass}.{SessionName.Replace("-", string.Empty)}.db";

            InitializeDatabase(initialization);
        }
    }
}
