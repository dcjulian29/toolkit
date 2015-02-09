using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Common.Logging;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ToolKit.Data.NHibernate.SessionFactories
{
    /// <summary>
    /// NHibernate Session Factory Abstract Class. This class is use by
    /// the "specific" Session Factory classes that provides specific
    /// DB (or persistence) configuration.
    /// </summary>
    public abstract class SessionFactoryBase : IDisposable
    {
        private static ILog _log = LogManager.GetLogger<SessionFactoryBase>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        protected SessionFactoryBase(string connectionString)
            : this(connectionString, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="createDatabase">if set to <c>true</c>, create the database.</param>
        protected SessionFactoryBase(string connectionString, bool createDatabase)
        {
            CheckIfConnectionStringIsConnectionKey(connectionString);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (assemblies.Length == 0)
            {
                throw new
                  InvalidOperationException("Could not load any assemblies in this AppDomain!");
            }

            var cfg = Fluently.Configure().Database(PersistenceConfigurer);

            cfg = assemblies
                .Where(ValidateAssembly)
                .Aggregate(
                    cfg,
                    (current, assembly) => 
                        current.Mappings(m => m.FluentMappings.AddFromAssembly(assembly)));

            if (createDatabase)
            {
                cfg = cfg.ExposeConfiguration(BuildSchema);
            }

            Factory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// Prevents a default instance of the
        /// <see cref="SessionFactoryBase"/> class from being created.
        /// </summary>
        private SessionFactoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SessionFactoryBase"/> class.
        /// </summary>
        ~SessionFactoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the connection string used for this session.
        /// </summary>
        /// <value>
        /// The connection string used for this session
        /// </value>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the NHibernate session factory.
        /// </summary>
        public ISessionFactory Factory { get; private set; }

        /// <summary>
        /// Gets the object that will configure the persistence layer.
        /// </summary>
        public abstract IPersistenceConfigurer PersistenceConfigurer { get; }

        /// <summary>
        /// Gets the NHibernate session.
        /// </summary>
        public ISession Session
        {
            get
            {
                return Factory.OpenSession();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || (Factory == null))
            {
                return;
            }

            Factory.Dispose();
            Factory = null;
        }

        private static void BuildSchema(global::NHibernate.Cfg.Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, true);
        }

        private static bool ValidateAssembly(Assembly assembly)
        {
            // Ignore assemblies from the GAC
            if (assembly.GlobalAssemblyCache)
            {
                return false;
            }

            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var baseType = type.BaseType;

                while (baseType != null)
                {
                    if (!String.IsNullOrEmpty(baseType.FullName)
                      && baseType.FullName.Contains("FluentNHibernate.Mapping.ClassMap"))
                    {
                        _log.DebugFormat("{0} mapping found.", type);
                        return true;
                    }

                    baseType = baseType.BaseType;
                }
            }

            _log.DebugFormat("{0} does not contain any mappings.", assembly.FullName);

            return false;
        }

        private void CheckIfConnectionStringIsConnectionKey(string connectionString)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[connectionString]))
            {
                _log.DebugFormat("\"{0}\" was loaded from Application Settings...", connectionString);
                ConnectionString = ConfigurationManager.AppSettings[connectionString];
            }

            var setting = ConfigurationManager.ConnectionStrings[connectionString];

            if (!String.IsNullOrEmpty(setting != null ? setting.ConnectionString : null))
            {
                _log.DebugFormat("\"{0}\" was loaded from Connection Settings...", connectionString);
                ConnectionString = setting.ConnectionString;
            }

            if (!String.IsNullOrWhiteSpace(ConnectionString))
            {
                return;
            }

            _log.Debug("ConnectionString is not an Application or Connection Setting, loaded directly...");
            ConnectionString = connectionString;
        }
    }
}
