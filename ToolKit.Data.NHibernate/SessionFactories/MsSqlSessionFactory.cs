using FluentNHibernate.Cfg.Db;

namespace ToolKit.Data.NHibernate.SessionFactories
{
    /// <summary>
    /// NHibernate Session Factory using Microsoft SQL Server 2012.
    /// </summary>
    public class MsSqlSessionFactory : SessionFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public MsSqlSessionFactory(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="createDatabase">if set to <c>true</c>, create the database.</param>
        public MsSqlSessionFactory(string connectionString, bool createDatabase)
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
                return MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString);
            }
        }
    }
}
