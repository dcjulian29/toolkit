using System;
using System.Reflection;
using System.Runtime.Caching;
using Common.Logging;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// A Abstract Implementation of A Database Handler Class Using NHibernate
    /// </summary>
    public abstract class NHibernateDatabaseBase : DatabaseBase
    {
        private static readonly object _cacheLock = new object();

        private static readonly CacheItemPolicy _cachePolicy = new CacheItemPolicy()
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1))
        };

        private static readonly ILog _log = LogManager.GetLogger<NHibernateDatabaseBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateDatabaseBase"/> class.
        /// </summary>
        /// <param name="assemblyContainingMappings">The assembly containing mappings.</param>
        protected NHibernateDatabaseBase(Assembly assemblyContainingMappings)
        {
            AssemblyContainingMappings = assemblyContainingMappings;
        }

        /// <summary>
        /// Gets the database instance.
        /// </summary>
        public static new NHibernateDatabaseBase Instance => _instance as NHibernateDatabaseBase;

        /// <summary>
        /// Gets the assembly containing mappings.
        /// </summary>
        public Assembly AssemblyContainingMappings { get; }

        /// <summary>
        /// Gets or sets the cache of Session Factories
        /// </summary>
        protected static ObjectCache Cache { get; set; } = MemoryCache.Default;

        /// <summary>
        /// This method will Initializes the database in a class that inherits from this base class.
        /// </summary>
        /// <param name="initialization">The action to preform to initialize database.</param>
        public abstract override void InitializeDatabase(Action initialization);

        /// <summary>
        /// Find and Return the Session Factory for the specified session name.
        /// </summary>
        /// <param name="sessionName">The name of the database session.</param>
        /// <returns>the Session Factory for the specified session name</returns>
        public ISessionFactory SessionFactory(string sessionName)
            => GetOrCreateSessionFactory(sessionName);

        /// <summary>
        /// Builds the database schema.
        /// </summary>
        /// <param name="config">The properties and mapping documents to be used.</param>
        protected static void BuildSchema(Configuration config) =>
                            new SchemaExport(config).Create(false, UnitTests);

        /// <summary>
        /// contains the details about the NHibernate Configuration
        /// </summary>
        /// <returns>the NHibernate Configuration</returns>
        protected abstract IPersistenceConfigurer DatabaseConfigurer();

        private ISessionFactory GetOrCreateSessionFactory(string sessionName)
        {
            if (Cache.Contains(sessionName))
            {
                _log.Debug($"Returning Existing session: {sessionName}");
            }
            else
            {
                lock (_cacheLock)
                {
                    if (!Cache.Contains(sessionName))
                    {
                        _log.Debug($"Creating a new session: {sessionName}");

                        var sessionFactory = Fluently.Configure()
                            .Database(DatabaseConfigurer)
                            .Mappings(m => m.FluentMappings.AddFromAssembly(AssemblyContainingMappings))
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();

                        Cache.Set(sessionName, sessionFactory, _cachePolicy);
                    }
                }
            }

            return Cache[sessionName] as ISessionFactory;
        }
    }
}
