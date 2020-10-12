using FluentNHibernate.Cfg.Db;

namespace ToolKit.Data.NHibernate.SessionFactories
{
    /// <summary>
    /// NHibernate Session Factory using Microsoft SQL Compact.
    /// </summary>
    public class MsSqlCeSessionFactory : SessionFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlCeSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public MsSqlCeSessionFactory(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlCeSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <param name="createDatabase">if set to <c>true</c>, create the database.</param>
        public MsSqlCeSessionFactory(string connectionString, bool createDatabase)
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
                return MsSqlCeConfiguration.Standard
                    .Dialect<global::NHibernate.Dialect.MsSqlCe40Dialect>()
                    .ConnectionString(ConnectionString);
            }
        }
    }
}
