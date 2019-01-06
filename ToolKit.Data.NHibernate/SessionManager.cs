using System;
using NHibernate;
using NHibernate.Proxy.DynamicProxy;
using ToolKit.Data.NHibernate.SessionFactories;
using IInterceptor = NHibernate.IInterceptor;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Handles creation and management of sessions. It is a singleton because building the initial
    /// session factory is very expensive.
    /// </summary>
    public class SessionManager : IDisposable
    {
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        /// <param name="factory">The session factory to use.</param>
        public SessionManager(ISessionFactory factory)
        {
            _sessionFactory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        /// <param name="factory">One of the ToolBox NHibernate Factory classes.</param>
        public SessionManager(SessionFactoryBase factory)
        {
            _sessionFactory = factory.Factory;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SessionManager"/> class from being created.
        /// </summary>
        private SessionManager()
        {
        }

        /// <summary>
        /// Finalizes an instance of the SessionManager class.
        /// </summary>
        ~SessionManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the interceptor to be used for new sessions.
        /// </summary>
        /// <value>The interceptor to be used for new sessions.</value>
        public IInterceptor Interceptor { private get; set; }

        /// <summary>
        /// Gets a session with or without an interceptor.
        /// </summary>
        public ISession Session => Interceptor == null
            ? _sessionFactory.OpenSession()
            : _sessionFactory.WithOptions().Interceptor(Interceptor).OpenSession();

        /// <inheritdoc/>
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
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
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _sessionFactory == null)
            {
                return;
            }

            _sessionFactory.Dispose();
            _sessionFactory = null;
        }
    }
}
