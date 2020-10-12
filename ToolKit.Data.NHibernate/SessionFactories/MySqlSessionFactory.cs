using FluentNHibernate.Cfg.Db;

namespace ToolKit.Data.NHibernate.SessionFactories
{
    /// <summary>
    /// NHibernate Session Factory using MySQL Server.
    /// </summary>
    public class MySqlSessionFactory : SessionFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public MySqlSessionFactory(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="createDatabase">if set to <c>true</c>, create the database.</param>
        public MySqlSessionFactory(string connectionString, bool createDatabase)
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
                return MySQLConfiguration.Standard.ConnectionString(ConnectionString);
            }
        }
    }
}
