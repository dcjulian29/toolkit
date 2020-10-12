using FluentNHibernate.Cfg.Db;

namespace ToolKit.Data.NHibernate.SessionFactories
{
    /// <summary>
    /// NHibernate Session Factory using Oracle Server.
    /// </summary>
    public class OracleSessionFactory : SessionFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OracleSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public OracleSessionFactory(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="createDatabase">if set to <c>true</c>, create the database.</param>
        public OracleSessionFactory(string connectionString, bool createDatabase)
            : base(connectionString, createDatabase)
        {
        }

        /// <summary>
        /// Gets the object that will configure the persistence layer.
        /// </summary>
        public override IPersistenceConfigurer PersistenceConfigurer
        {
            get
            {
                return OracleClientConfiguration.Oracle10.ConnectionString(ConnectionString);
            }
        }
    }
}
